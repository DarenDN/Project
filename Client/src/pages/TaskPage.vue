<template>
  <div class="container">
    <h5 class="title">{{ store?.currentTask?.title }}</h5>
    <div style="display: flex">
      <q-avatar
        :size="xs"
        color="primary"
        text-color="white"
        icon="perm_identity"
      />
      <h6 class="title">Created by {{ store?.currentTask?.author }}</h6>
    </div>

    <div class="q-pa-md row items-start q-gutter-md">
      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Type</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">{{
          store?.currentTask?.type
        }}</q-card-section>
      </q-card>

      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Status</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">{{
          store?.currentTask?.status
        }}</q-card-section>
      </q-card>

      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Estimate time</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">{{
          store?.currentTask?.estimationTime || 'none'
        }}</q-card-section>
      </q-card>

      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Estimate point</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">{{
          store?.currentTask?.estimationPoint || 'none'
        }}</q-card-section>
      </q-card>

      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Performer</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">{{
          store?.currentTask?.performer
        }}</q-card-section>
      </q-card>
    </div>

    <div class="separator">
      <q-separator inset />
    </div>
    <h6 class="title">Description</h6>
    <p class="description">
      {{ store?.currentTask?.description }}
    </p>
  </div>

  <q-fab
    class="fab"
    vertical-actions-align="center"
    color="primary"
    icon="keyboard_arrow_up"
    direction="up"
  >
    <q-fab-action
      color="secondary"
      @click="createDialog = true"
      icon="edit"
      label="Edit sprint"
    />
    <q-fab-action
      color="secondary"
      @click="console.log('set as mine')"
      icon="check_circle_outline"
      label="Set as mine"
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

if (!store.sprint) {
  router.push("/home/empty-sprint");
}

if (!store.currentTask) {
  router.push("/home/dashboard");
}

const createDialog = ref(false);
</script>
<style scoped>
.title {
  margin: 10px;
}
.container {
  padding: 20px;
}
.separator {
  margin: 20px;
}
.card-separator {
  margin: 5px;
}
.card {
  width: 150px;
  text-align: center;
}
.fab {
  position: absolute;
  bottom: 50px;
  right: 50px;
}
.description {
  background-color: aliceblue;
  padding: 10px;
  min-height: 200px;
}
</style>
