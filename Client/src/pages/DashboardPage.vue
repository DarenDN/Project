<template>
  <div class="container">
    <div class="column" v-for="taskStatus in taskStatuses" :key="taskStatus">
      <h6 class="column-header">{{ taskStatus }}</h6>
      <div class="content">
        <div
          class="card-wrapper"
          v-for="task in tasks.filter((x) => x.status === taskStatus)"
          :key="task.title"
          @click="openTask(task)"
        >
          <q-card
            class="task-card text-white"
            style="background-color: #1976d2"
          >
            <q-card-section>
              <div>
                {{ task.title }}
              </div>
              <q-badge transparent align="middle" color="orange">
                {{ getUserNameById(task.creator) }}
              </q-badge>
            </q-card-section>

            <q-separator class="card-separator" inset />
          </q-card>
        </div>
      </div>
    </div>
  </div>

  <q-fab
    class="fab"
    vertical-actions-align="center"
    color="primary"
    icon="keyboard_arrow_up"
    direction="up"
  >
    <q-fab-action
      color="primary"
      @click="createDialog = true"
      icon="add_circle"
      label="Create new"
    />
  </q-fab>
  <TaskCreationWindow v-model="createDialog"></TaskCreationWindow>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { store } from "stores/store";
import { httpClient } from "src/utils/httpClient";
import { mask } from "src/utils/mask";
import { useRouter } from "vue-router";
import TaskCreationWindow from "src/components/windows/TaskCreationWindow.vue";

const router = useRouter();
const usersMapping = ref(new Map());
const createDialog = ref(false);

if (!store.sprint) {
  router.push("/home/empty-sprint");
}

onMounted(() => {
  if (store.sprint) {
    mask.show();
    httpClient
      .get(httpClient.ProjectManagementServicePath, "State/GetStatesAsync")
      .then((resp) => resp.json())
      .then((result) => {
        result.states.sort((a, b) => a.order - b.order);
        taskStatuses.value = result.states.map((x) => x.name);
      })
      .then((result) =>
        httpClient.get(
          httpClient.ProjectManagementServicePath,
          "Task/GetSprintTasksAsync"
        )
      )
      .then((resp) => resp.json())
      .then((result) => {
        tasks.value = result.tasks;
      })
      .then((result) => {
        const users = tasks.value
          .map((task) => [task.creator, task.actor])
          .flat();
        return httpClient.put(
          httpClient.IdentityManagementPath,
          "IdentityManagement/GetShortUserInfosAsync",
          users
        );
      })
      .then((resp) => resp.json())
      .then((result) => {
        if (result) {
          result.forEach((user) => {
            usersMapping.value.set(user.id, user);
          });
        }
        mask.hide();
      });
  }
});

const taskStatuses = ref([]);

const tasks = ref([]);

function openTask(task) {
  store.currentTask = task;

  mask.show();

  httpClient
    .post(
      httpClient.ProjectManagementServicePath,
      `Task/GetTaskAsync?taskId=${task.id}`,
      {}
    )
    .then((response) => response.json())
    .then((task) => {
      store.currentTask = task;
      store.currentTask.author = getUserNameById(task.creatorId);
      store.currentTask.performer = getUserNameById(task.performerId);
      mask.hide();
    });

  router.push("/home/dashboard/task");
}

function getUserNameById(id) {
  const user = usersMapping.value.get(id);

  return user ? `${user?.firstName} ${user?.lastName}` : "none";
}
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  padding: 20px;
}
.content {
  background-color: aliceblue;
  border-radius: 5px;
}
.column {
  margin: 10px;
  flex-basis: 0;
  flex-grow: 1;
}
.column-header {
  margin: 5px;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
}
.card-wrapper {
  width: 100%;
  padding: 20px;
}
.fab {
  position: fixed;
  bottom: 50px;
  right: 50px;
}
</style>
