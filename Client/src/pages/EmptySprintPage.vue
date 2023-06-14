<template>
  <div class="container">
    <div class="text text-h2" style="opacity: 0.4">No sprint found</div>
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
  <SprintCreationWindow v-model="createDialog"></SprintCreationWindow>
</template>

<script setup>
import { ref } from "vue";
import SprintCreationWindow from "src/components/windows/SprintCreationWindow.vue";

const createDialog = ref(false);
</script>

<style scoped>
.container {
  display: flex;
  height: 500px;
  align-items: center;
  position: relative;
}
.text {
  width: 100%;
  flex-grow: 1;
  text-align: center;
}
.fab {
  position: absolute;
  bottom: 50px;
  right: 50px;
}
</style>
