<template>
  <div class="container">
    <div class="column" v-for="taskStatus in taskStatuses" :key="taskStatus">
      <h6 class="column-header">{{ taskStatus }}</h6>
      <div class="content">
        <div
          class="card-wrapper"
          v-for="task in tasks.filter((x) => x.status === taskStatus)"
          :key="task.name"
        >
          <q-card
            class="task-card text-white"
            style="background-color: #1976d2"
          >
            <q-card-section>
              <div class="text-h6">
                {{ taskTitle }}
                <q-badge transparent align="middle" color="orange">
                  {{ task.name }}
                </q-badge>
              </div>
            </q-card-section>
            <q-separator class="card-separator" inset />
            <q-card-section class="q-pt-none">{{ description }}</q-card-section>
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
import TaskCreationWindow from 'src/components/windows/TaskCreationWindow.vue'

const router = useRouter();

const createDialog = ref(false);

if (!store.sprint) {
  router.push("/home/emptySprint");
}

const taskStatuses = store?.sprint?.taskStatuses || ['new', 'done'];
const taskTitle = "Task title";
const description = "some description, that makes no sense";

const tasks = [
  {
    name: "kek",
    status: "development",
  },
  {
    name: "kek2",
    status: "review",
  },
  {
    name: "kek3",
    status: "review",
  },
  {
    name: "kek4",
    status: "development",
  },
  {
    name: "kek5",
    status: "done",
  },
  {
    name: "kek6",
    status: "done",
  },
  {
    name: "kek7",
    status: "done",
  },
  {
    name: "kek8",
    status: "new",
  },
  {
    name: "kek9",
    status: "keking",
  },
  {
    name: "kek10",
    status: "rofl",
  },
];
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
