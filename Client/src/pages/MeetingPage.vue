<template>
  <div class="container">
    <div class="header">
      <h6 class="label">Current story: {{ selectedTask?.name || "None" }}</h6>
      <q-separator spaced />
      <q-chip color="primary" text-color="white" icon="schedule">
        Estimate:
        {{
          (selectedTask?.status == "voted" && selectedTask?.estimate) || "None"
        }}
      </q-chip>
      <q-chip color="primary" text-color="white" icon="stacked_bar_chart">
        Point:
        {{ (selectedTask?.status == "voted" && selectedTask?.point) || "None" }}
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
              currentStatus === CHOOSE_STATUS && user.value !== '?',
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
                    <q-icon name="schedule" />: {{ user.value }} 
                    <q-icon name="stacked_bar_chart" />: {{ user.value }}
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
            v-for="task in sprintBacklog"
            v-bind:key="task.name"
            clickable
            v-ripple
            :active="selectedTask?.name === task.name"
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
              <q-item-label caption>{{ task.status }}</q-item-label>
            </q-item-section>

            <q-item-section v-show="isAdmin" side @click="toProduct(task)">
              <q-icon name="remove_circle_outline" />
            </q-item-section>
          </q-item>

          <q-separator spaced />
          <q-item-label header>Product backlog</q-item-label>

          <q-item
            v-for="task in productTasks"
            v-bind:key="task.name"
            active-class="selected-task"
          >
            <q-item-section avatar top>
              <q-avatar icon="assignment" color="grey" text-color="white" />
            </q-item-section>

            <q-item-section>
              <q-item-label lines="1">{{ task.name }}</q-item-label>
            </q-item-section>

            <q-item-section v-show="isAdmin" side @click="toSprint(task)">
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
    v-model="createDialog"
  ></MeetingTaskWindow>
</template>

<script setup>
import { useQuasar } from "quasar";
import { ref, onMounted } from "vue";
import { store } from "stores/store";
import { useRouter } from "vue-router";
import MeetingTaskWindow from "src/components/windows/MeetingTaskWindow.vue";

const CHOOSE_STATUS = "CHOOSE_STATUS";
const OPENED_STATUS = "OPENED_STATUS";
const FINAL_STATUS = "FINAL_STATUS";

const q = useQuasar();
const router = useRouter();
const isAdmin = store.user.isAdmin;

const windowTitle = ref("Set task estimate");
const currentStatus = ref(CHOOSE_STATUS);
const createDialog = ref(false);

const users = ref([
  { id: 1, name: "John", value: 5 },
  { id: 2, name: "Sarah", value: 12 },
  { id: 3, name: "David", value: "?" },
  { id: 4, name: "Emily", value: 8 },
  { id: 5, name: "Michael", value: 1 },
  { id: 6, name: "Emma", value: 15 },
  { id: 7, name: "Jacob", value: "?" },
  { id: 8, name: "Olivia", value: 3 },
  { id: 9, name: "Ethan", value: 11 },
  { id: 10, name: "Ava", value: "?" },
]);

const productTasks = ref([
  {
    name: "Implement new login page",
    status: "not voted",
    point: 3,
    estimate: "5h",
  },
  {
    name: "Fix broken search functionality",
    status: "voted",
    point: 2,
    estimate: "3h",
  },
  {
    name: "Improve site performance",
    status: "not voted",
    point: 4,
    estimate: "8h",
  },
  {
    name: "Add product recommendations to homepage",
    status: "not voted",
    point: 2,
    estimate: "2h",
  },
  {
    name: "Create user onboarding flow",
    status: "voted",
    point: 5,
    estimate: "10h",
  },
  {
    name: "Redesign checkout process",
    status: "not voted",
    point: 6,
    estimate: "12h",
  },
  {
    name: "Update pricing display logic",
    status: "voted",
    point: 1,
    estimate: "1h",
  },
  {
    name: "Integrate with third-party API",
    status: "not voted",
    point: 5,
    estimate: "7h",
  },
  {
    name: "Implement internationalization support",
    status: "not voted",
    point: 3,
    estimate: "6h",
  },
  {
    name: "Create mobile app version",
    status: "not voted",
    point: 7,
    estimate: "15h",
  },
]);
const sprintBacklog = ref([]);
const selectedTask = ref(null);

function openCards() {
  currentStatus.value = OPENED_STATUS;
  windowTitle.value = "Set final task estimate";
}

function revote() {
  currentStatus.value = CHOOSE_STATUS;
  windowTitle.value = "Set task estimate";
  selectedTask.value.status = "not voted";
}

function leave() {
  if (isAdmin) {
  }

  router.push("/home");
}

function toSprint(element) {
  productTasks.value = productTasks.value.filter(
    (x) => x.name !== element.name
  );
  sprintBacklog.value.push(element);
}

function toProduct(element) {
  sprintBacklog.value = sprintBacklog.value.filter(
    (x) => x.name !== element.name
  );
  productTasks.value.push(element);
}

function submitVotes() {
  createDialog.value = true;
  currentStatus.value = FINAL_STATUS;
  selectedTask.value.status = "voted";
}

function selectTask(task) {
  if (!isAdmin) return;

  if (selectedTask.value?.name !== task.name) {
    currentStatus.value = CHOOSE_STATUS;
  }
  selectedTask.value = task;
  windowTitle.value = "Set task estimate";
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
