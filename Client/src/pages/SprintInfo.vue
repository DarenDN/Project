<template>
  <div class="container">
    <h5 class="title">{{ store?.sprint?.sprintName }}</h5>
    <div class="q-pa-md row items-start q-gutter-md">
      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Date created</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">{{ store?.sprint?.dateCreated }}</q-card-section>
      </q-card>

      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Start date</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">
          {{ store?.sprint?.startDate }}</q-card-section
        >
      </q-card>

      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">End date</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">
          {{ store?.sprint?.endDate }}</q-card-section
        >
      </q-card>
    </div>

    <div class="separator">
      <q-separator inset />
    </div>
    <h6 class="title">Description</h6>
    <div class="description">
      {{ store?.sprint?.description }}
    </div>
  </div>

  <q-fab
    v-show="store?.user?.isAdmin"
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
      v-show="store?.user?.isAdmin"
      color="primary"
      @click="finishSprint()"
      icon="check_circle_outline"
      label="Finish sprint"
    />
  </q-fab>
  <SprintCreationWindow v-model="createDialog"></SprintCreationWindow>
</template>

<script setup>
import { ref } from "vue";
import { store } from "stores/store";
import { useRouter } from "vue-router";
import SprintCreationWindow from "src/components/windows/SprintCreationWindow.vue";

const router = useRouter();
const createDialog = ref(false);

if (!store.sprint) {
  router.push("/home/empty-sprint");
}

function finishSprint() {
  store.sprint = null;
  router.push("/home/empty-sprint");
}
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
  width: 200px;
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
