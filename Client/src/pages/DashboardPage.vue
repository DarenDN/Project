<template>
  <div class="container">
    <div class="column" v-for="taskType in taskTypes" :key="taskType">
      <h6 class="column-header">{{ taskType }}</h6>
      <div class="content">
        <div
          class="card-wparrer"
          v-for="task in tasks.filter((x) => x.type === taskType)"
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
</template>

<script setup>
const taskTypes = ["development", "review", "done"];
const taskTitle = "Task title";
const description = "some description, that makes no sense";

const tasks = [
  {
    name: "kek",
    type: "development",
  },
  {
    name: "kek2",
    type: "review",
  },
  {
    name: "kek3",
    type: "review",
  },
  {
    name: "kek4",
    type: "development",
  },
  {
    name: "kek5",
    type: "done",
  },
  {
    name: "kek6",
    type: "done",
  },
  {
    name: "kek7",
    type: "done",
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
.card-wparrer {
  width: 100%;
  padding: 20px;
}
</style>
