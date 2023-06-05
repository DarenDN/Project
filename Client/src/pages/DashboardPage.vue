<template>
  <div class="container">
    <div class="column" v-for="taskStatus in taskStatuses" :key="taskStatus">
      <h6 class="column-header">{{ taskStatus }}</h6>
      <div class="content">
        <div
          class="card-wrapper"
          v-for="task in tasks.filter((x) => x.status === taskStatus)"
          :key="task.taskName"
          @click="openTask(task)"
        >
          <q-card
            class="task-card text-white"
            style="background-color: #1976d2"
          >
            <q-card-section>
              <div class="text-h6">
                {{ task.taskName }}
                <q-badge transparent align="middle" color="orange">
                  {{ 'task' + tasks.indexOf(task.taskName) }}
                </q-badge>
              </div>
            </q-card-section>
            <q-separator class="card-separator" inset />
            <q-card-section class="q-pt-none">{{ task.description }}</q-card-section>
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
import { ref } from "vue";
import { store } from "stores/store";
import { useRouter } from "vue-router";
import TaskCreationWindow from "src/components/windows/TaskCreationWindow.vue";

const router = useRouter();

const createDialog = ref(false);

if (!store.sprint) {
  router.push("/home/empty-sprint");
}

const taskStatuses = store?.sprint?.taskStatuses || ["new", "done"];

const tasks = [
  {
    timeEstimate: "1 hour",
    author: "Alex.J.J",
    status: "done",
    type: "feature",
    performer: "Charlie.L.W",
    taskName: "Update user profile page",
    description: "Add fields for phone number and address",
  },
  {
    timeEstimate: "20 hours",
    author: "Dave.A.L",
    status: "new",
    type: "bug",
    performer: "Alice.B.J",
    taskName: "Fix login error message",
    description: "Change message to be more descriptive",
  },
  {
    timeEstimate: "2 hours",
    author: "Eve.N.S",
    status: "review",
    type: "feature",
    performer: "Bob.R.T",
    taskName: "Implement payment gateway",
    description: "Enable users to purchase premium features",
  },
  {
    timeEstimate: "30 minutes",
    author: "Charlie.K.G",
    status: "in progress",
    type: "bug",
    performer: "Dave.A.L",
    taskName: "Fix broken link on homepage",
    description: "Link to about page goes to 404",
  },
  {
    timeEstimate: "5 hours",
    author: "Bob.R.T",
    status: "in progress",
    type: "feature",
    performer: "Eve.N.S",
    taskName: "Create admin dashboard",
    description: "Allow admins to manage users and content",
  },
  {
    timeEstimate: "1 hour",
    author: "Alice.B.J",
    status: "new",
    type: "bug",
    performer: "Charlie.K.G",
    taskName: "Fix broken image on product page",
    description: "Image resolution is too low",
  },
  {
    timeEstimate: "2 hours",
    author: "Charlie.L.W",
    status: "done",
    type: "bug",
    performer: "Bob.R.T",
    taskName: "Fix broken link on about page",
    description: "Link to careers page goes to 404",
  },
  {
    timeEstimate: "10 minutes",
    author: "Dave.A.L",
    status: "done",
    type: "feature",
    performer: "Eve.N.S",
    taskName: "Add social media links to footer",
    description: "Include links to Facebook, Twitter, and Instagram",
  },
  {
    timeEstimate: "20 hours",
    author: "Bob.R.T",
    status: "review",
    type: "bug",
    performer: "Alice.B.J",
    taskName: "Fix checkout process",
    description: "Payments fail at step 3",
  },
  {
    timeEstimate: "5 hours",
    author: "Eve.N.S",
    status: "new",
    type: "feature",
    performer: "Charlie.L.W",
    taskName: "Implement chat functionality",
    description: "Allow users to send messages to each other",
  },
  {
    timeEstimate: "2 hours",
    author: "Alex.J.J",
    status: "done",
    type: "bug",
    performer: "Charlie.L.W",
    taskName: "Fix broken search functionality",
    description: "Search results are not displaying correctly",
  },
  {
    timeEstimate: "10 minutes",
    author: "Dave.A.L",
    status: "new",
    type: "feature",
    performer: "Alice.B.J",
    taskName: "Add email sign-up form",
    description: "Allow users to subscribe to newsletter",
  },
  {
    timeEstimate: "1 hour",
    author: "Eve.N.S",
    status: "review",
    type: "bug",
    performer: "Bob.R.T",
    taskName: "Fix broken pagination on blog",
    description: "Page numbers are not displaying correctly",
  },
  {
    timeEstimate: "5 hours",
    author: "Charlie.K.G",
    status: "in progress",
    type: "feature",
    performer: "Dave.A.L",
    taskName: "Implement user rating system",
    description: "Allow users to rate content and leave comments",
  },
  {
    timeEstimate: "20 hours",
    author: "Bob.R.T",
    status: "done",
    type: "bug",
    performer: "Eve.N.S",
    taskName: "Fix broken checkout process",
    description: "Payments fail at step 2",
  },
];

function openTask(task) {
  store.currentTask = task;
  router.push("/home/dashboard/task");
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
  flex-grow: 1;
  margin: 10px;
}
.column-header {
  margin: 5px;
}
.card-wrapper {
  width: 100%;
  padding: 20px;
}
.fab {
  position: absolute;
  bottom: 50px;
  right: 50px;
}
</style>
