<template>
  <div class="container">
    <div class="header">
      <h6 class="label">Current story: {{ selectedTask?.name || "None" }}</h6>
      <q-separator spaced />
      <q-chip color="primary" text-color="white" icon="schedule">
        Estimate:
        {{ selectedTask?.finalEvaluation?.evaluationTime || "None" }}
      </q-chip>
      <q-chip color="primary" text-color="white" icon="stacked_bar_chart">
        Point:
        {{ selectedTask?.finalEvaluation?.evaluationPoints || "None" }}
      </q-chip>
    </div>
    <div class="main-content">
      <div class="card-container">
        <q-card
          v-for="user in users"
          v-bind:key="user.id"
          :class="{
            card: true,
            'card-closen-not-voted': currentStatus === CHOOSE_STATUS,
            'card-closen-voted':
              currentStatus === CHOOSE_STATUS && getUserEvaluation(user),
          }"
          bordered
        >
          <q-slide-transition>
            <div v-show="currentStatus === CHOOSE_STATUS">
              <q-card-section>
                <div class="text-h6">{{ user.name }}</div>
              </q-card-section>
            </div>
          </q-slide-transition>

          <q-slide-transition>
            <div v-show="currentStatus !== CHOOSE_STATUS">
              <q-card-section>
                <div class="text-h6">{{ user.name }}</div>
              </q-card-section>
              <q-separator class="card-separator" inset />
              <q-card-section>
                <div class="text-h6">
                  <div v-show="user.value !== '?'">
                    <q-icon name="schedule" />:
                    {{ getUserEvaluation(user)?.evaluationPoints }} 
                    <q-icon name="stacked_bar_chart" />:
                    {{ getUserEvaluation(user)?.evaluationTime }}
                  </div>
                  <div v-show="user.value === '?'">-</div>
                </div>
              </q-card-section>
            </div>
          </q-slide-transition>
        </q-card>
      </div>
      <div class="tasks">
        <q-list
          bordered
          padding
          class="rounded-borders"
          style="max-width: 350px"
        >
          <q-item-label header>Sprint backlog</q-item-label>

          <q-item
            v-for="task in tasks.sprintBacklog"
            v-bind:key="task.name"
            clickable
            v-ripple
            :active="isAdmin && selectedTask?.id === task.id"
            @click="selectTask(task)"
            active-class="selected-task"
          >
            <q-item-section avatar top>
              <q-avatar
                icon="assignment"
                color="secondary"
                text-color="white"
              />
            </q-item-section>

            <q-item-section>
              <q-item-label lines="1">{{ task.name }}</q-item-label>
              <q-item-label caption>{{
                task.voted ? "Voted" : "Not voted"
              }}</q-item-label>
            </q-item-section>

            <q-item-section
              v-show="isAdmin"
              side
              @click="changeTaskType(task, 0)"
            >
              <q-icon name="remove_circle_outline" />
            </q-item-section>
          </q-item>

          <q-separator spaced />
          <q-item-label header>Product backlog</q-item-label>

          <q-item
            v-for="task in tasks.productTasks"
            v-bind:key="task.id"
            active-class="selected-task"
          >
            <q-item-section avatar top>
              <q-avatar icon="assignment" color="grey" text-color="white" />
            </q-item-section>

            <q-item-section>
              <q-item-label lines="1">{{ task.name }}</q-item-label>
            </q-item-section>

            <q-item-section
              v-show="isAdmin"
              side
              @click="changeTaskType(task, 1)"
            >
              <q-icon name="add_circle_outline" />
            </q-item-section>
          </q-item>
        </q-list>
      </div>
    </div>
  </div>

  <q-fab
    class="fab"
    vertical-actions-align="center"
    color="primary"
    icon="keyboard_arrow_left"
    direction="left"
  >
    <q-fab-action
      color="red"
      @click="leave()"
      icon="exit_to_app"
      :label="isAdmin ? 'Finish meeting' : 'Leave'"
    />
    <q-fab-action
      v-show="selectedTask && isAdmin && currentStatus === CHOOSE_STATUS"
      color="primary"
      @click="openCards()"
      icon="check_circle_outline"
      label="Open cards"
    />
    <q-fab-action
      v-show="selectedTask && isAdmin && currentStatus === OPENED_STATUS"
      color="primary"
      @click="submitVotes()"
      icon="check_circle_outline"
      label="Submit votes"
    />
    <q-fab-action
      v-show="selectedTask && isAdmin && currentStatus === FINAL_STATUS"
      color="secondary"
      @click="revote()"
      icon="edit"
      label="Revote"
    />
    <q-fab-action
      v-show="selectedTask && currentStatus === CHOOSE_STATUS"
      color="secondary"
      @click="createDialog = true"
      icon="edit"
      label="Current task"
    />
  </q-fab>

  <MeetingTaskWindow
    :title="windowTitle"
    :current-task="selectedTask"
    :endpont="endpont"
    :callback="callback"
    v-model="createDialog"
  ></MeetingTaskWindow>
</template>

<script setup>
import { useQuasar } from "quasar";
import { ref, onMounted, reactive, toRefs } from "vue";
import { store } from "stores/store";
import { useRouter } from "vue-router";
import { httpClient } from "src/utils/httpClient";
import { hub } from "src/utils/hub";
import MeetingTaskWindow from "src/components/windows/MeetingTaskWindow.vue";
import { mask } from "src/utils/mask";

const CHOOSE_STATUS = "CHOOSE_STATUS";
const OPENED_STATUS = "OPENED_STATUS";
const FINAL_STATUS = "FINAL_STATUS";

const q = useQuasar();
const router = useRouter();
const isAdmin = store.user.isAdmin;

const windowTitle = ref("Set task estimate");
const currentStatus = ref(CHOOSE_STATUS);
const createDialog = ref(false);
const users = ref([]);
const selectedTask = ref(null);
const tasks = reactive({ productTasks: [], sprintBacklog: [] });
const callback = ref(null);
const endpont = ref(null);
const refTasks = toRefs(tasks);

onMounted(function () {
  mask.show("loading");
  httpClient
    .get(httpClient.MeetingServicePath, "Meeting/IsMeetingExistAsync")
    .then((response) => response.json())
    .then((result) => {
      if (result.exists) {
        q.notify({
          message: "Joining meeting",
          color: "primary",
        });
        return joinMeeting();
      } else if (store.user.isAdmin) {
        q.notify({
          message: "Creating meeting",
          color: "primary",
        });
        return createMeeting();
      } else {
        router.push("/home");
        q.notify({
          message: "Meeting doesn't exist",
          color: "primary",
        });
      }
    })
    .then((response) => mask.hide());
});

function createMeeting() {
  return httpClient
    .get(httpClient.ProjectManagementServicePath, "Task/GetTasksBacklogAsync")
    .then((response) => response.json())
    .then((result) => {
      return result.tasksBacklog;
    })
    .then((tasks) => {
      const data = {
        userName: store.user.firstName,
        projectId: store.project.id,
        tasks,
      };
      return httpClient.post(
        httpClient.MeetingServicePath,
        "Meeting/CreateMeetingAndJoinAsync",
        data
      );
    })
    .then((response) => response.json())
    .then((result) => {
      initData(result);
    });
}

function joinMeeting() {
  return httpClient
    .post(
      httpClient.MeetingServicePath,
      `Meeting/JoinMeetingAsync?userName=${store.user.firstName}`
    )
    .then((response) => response.json())
    .then((result) => {
      initData(result);
    });
}

function initData(data) {
  hub.start();
  users.value = data.participants;
  data.backlog.forEach((element) => {
    if (element.backlogType == 1) {
      refTasks.sprintBacklog.value.push(element);
    } else {
      refTasks.productTasks.value.push(element);
    }
  });

  hub.on("UserJoinedMeetingAsync", (data) => {
    users.value = [...users.value, { ...data.participantDto, voted: false }];
  });
  hub.on("UserLeavedMeetingAsync", (data) => {
    users.value = users.value.filter(function (x) {
      return x.id !== data.participantId;
    });
  });
  hub.on("ChangeActiveTaskAsync", (data) => {
    if (selectedTask.value?.id === data.id) return;

    selectedTask.value = data;

    if (data.finalEvaluation) {
      currentStatus.value = FINAL_STATUS;
      const show =
        selectedTask.value && isAdmin && currentStatus.value === FINAL_STATUS;
    } else if (data.opened) {
      setOpenedCards();
    } else {
      currentStatus.value = CHOOSE_STATUS;
      windowTitle.value = "Set task estimate";
      endpont.value = "Meeting/UpdateUserEvaluationAsync";
      callback.value = () => {
        mask.hide();
      };
    }
  });
  hub.on("ChangeTaskBacklogTypeAsync", (data) => {
    if (data.backlogType === 1) {
      const deletingTask = refTasks.productTasks.value.filter(
        (x) => x.id === data.taskId
      )[0];
      refTasks.productTasks.value = refTasks.productTasks.value.filter(
        (x) => x.id !== data.taskId
      );
      refTasks.sprintBacklog.value.push(deletingTask);
    } else {
      const deletingTask = refTasks.sprintBacklog.value.filter(
        (x) => x.id === data.taskId
      )[0];
      refTasks.sprintBacklog.value = refTasks.sprintBacklog.value.filter(
        (x) => x.id !== data.taskId
      );
      refTasks.productTasks.value.push(deletingTask);
    }
  });
  hub.on("UpdateUserEvaluationAsync", (data) => {
    console.log(data);
    selectedTask.value.evaluationByUsers = data.evaluationByUsers;

    const user = users.value.filter((x) => x.id === data.participantDto.id)[0];

    user["evaluationDto"] = data.evaluationDto;
  });
  hub.on("DeleteMeetingAsync", (data) => {
    hub.stop();
    router.push("/home");
    q.notify({
      message: "Meeting has been ended",
      color: "primary",
    });
  });
  hub.on("ShowEvaluationsAsync", (data) => {
    setOpenedCards();
  });
  hub.on("EvaluateTaskFinalAsync", (data) => {
    currentStatus.value = FINAL_STATUS;
    windowTitle.value = "Set task estimate";
    selectedTask.value.finalEvaluation = {};
    selectedTask.value.finalEvaluation.evaluationPoints = data.evaluationPoints;
    selectedTask.value.finalEvaluation.evaluationTime = data.evaluationTime;
  });
  hub.on("ReevaluateAsync", (data) => {
    selectedTask.value = data;

    if (data.finalEvaluation) {
      currentStatus.value = FINAL_STATUS;
    } else if (data.opened) {
      setOpenedCards();
    } else {
      currentStatus.value = CHOOSE_STATUS;
      windowTitle.value = "Set task estimate";
      endpont.value = "Meeting/UpdateUserEvaluationAsync";
      callback.value = () => {
        mask.hide();
      };
    }
  });
}

function openCards() {
  httpClient.put(
    httpClient.MeetingServicePath,
    "Meeting/ShowEvaluationsAsync",
    { taskId: selectedTask.value.id }
  );
}

function setOpenedCards() {
  currentStatus.value = OPENED_STATUS;
  windowTitle.value = "Set final task estimate";
}

function revote() {
  httpClient.post(httpClient.MeetingServicePath, "Meeting/ReevaluateAsync", {
    taskId: selectedTask.value.id,
  });
}

function leave() {
  if (isAdmin) {
    httpClient
      .get(httpClient.MeetingServicePath, "Meeting/GetFinalEvaluationsAsync")
      .then((response) => response.json())
      .then((result) => {
        return httpClient.put(
          httpClient.ProjectManagementServicePath,
          "Task/UpdateTasksAsync",
          {
            backlogTaskDtos: result.backlogTasksDto,
          }
        );
      })
      .then((result) => {
        return httpClient.post(
          httpClient.MeetingServicePath,
          "Meeting/DeleteMeetingAsync"
        );
      });
  } else {
    httpClient.post(httpClient.MeetingServicePath, "Meeting/LeaveMeetingAsync");
  }
  hub.stop();
  router.push("/home");
}

function getUserEvaluation(user) {
  if (selectedTask?.value?.evaluationByUsers) {
    return selectedTask.value.evaluationByUsers[user.id];
  }
}

function changeTaskType(element, backlogType) {
  const taskData = {
    taskId: element.id,
    backlogType,
  };
  httpClient
    .post(
      httpClient.MeetingServicePath,
      "Meeting/ChangeTaskBacklogTypeAsync",
      taskData
    )
    .then((response) => /* Mask TODO */ {});
}

function submitVotes() {
  createDialog.value = true;

  endpont.value = "Meeting/EvaluateTaskFinalAsync";
  callback.value = () => {
    mask.hide();
  };
}

function selectTask(task) {
  if (!isAdmin) return;

  const taskData = {
    taskId: task.id,
  };

  httpClient.put(
    httpClient.MeetingServicePath,
    "Meeting/ChangeActiveTaskAsync",
    taskData
  );
}
</script>

<style scoped>
.header {
  padding-left: 30px;
}
.label {
  margin: 10px 0px;
}
.chip {
  font-size: 15px;
}
.card-container {
  display: flex;
  width: 80%;
  flex-wrap: wrap;
  justify-content: space-between;
  padding: 20px;
}

.main-content {
  display: flex;
}
.tasks {
  width: 20%;
}
.card {
  width: 200px;
  text-align: center;
  width: calc(33.33% - 10px);
  margin-bottom: 10px;
  max-height: 150px;
}
.card-closen-not-voted {
  background-color: #c2c2c2;
}
.card-closen-voted {
  background-color: #2196f3;
}
.selected-task {
  color: white;
  background: #1976d2;
}
.fab {
  position: fixed;
  bottom: 50px;
  right: 50px;
}
</style>
