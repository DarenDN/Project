<template>
  <BacklogView source="Task/GetSprintTasksAsync"></BacklogView>
</template>

<script setup>
import { store } from "stores/store";
import { useRouter } from "vue-router";
import BacklogView from "src/components/BacklogView.vue";

const router = useRouter();


if (!store.sprint) {
  router.push("/home/empty-sprint");
}
</script>

<style></style>
