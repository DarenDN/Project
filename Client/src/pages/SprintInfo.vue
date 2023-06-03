<template>
  <div class="container">
    <h6 class="title">{{ sprintName }}</h6>
    <div class="q-pa-md row items-start q-gutter-md">
      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Date created</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">{{ dateCreated }}</q-card-section>
      </q-card>

      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">Start date</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none"> {{ dateStart }}</q-card-section>
      </q-card>

      <q-card class="card" bordered>
        <q-card-section>
          <div class="text-h6">End date</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none"> {{ dateEnd }}</q-card-section>
      </q-card>
    </div>

    <div class="separator">
      <q-separator inset />
    </div>
    <p class="text-h6">
      {{ discription }}
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
import SprintCreationWindow from 'src/components/windows/SprintCreationWindow.vue'

const router = useRouter();

if (!store.sprint) {
  router.push("/home/emptySprint");
}

const discription = ref(store?.sprint?.discription);
const sprintName = ref(store?.sprint?.sprintName);
const dateCreated = ref(store?.sprint?.dateCreated);
const dateStart = ref(store?.sprint?.startDate);
const dateEnd = ref(store?.sprint?.endDate);
const createDialog = ref(false);

function finishSprint() {
  store.sprint = null;
  router.push("/home/emptySprint");
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
</style>
