<template>
  <BacklogView :records="records"></BacklogView>
</template>

<script setup>
import { store } from "stores/store";
import BacklogView from "src/components/BacklogView.vue";

const records = [
  {
    name: "Implement login feature",
    type: "new",
    status: "feature",
    author: "John",
  },
  {
    name: "Fix pagination bug",
    type: "done",
    status: "bug",
    author: "Sarah",
  },
  {
    name: "Add unit tests for user profile page",
    type: "new",
    status: "feature",
    author: "Michael",
  },
  {
    name: "Refactor code for better maintainability",
    type: "new",
    status: "feature",
    author: "David",
  },
  {
    name: "Investigate performance issue on dashboard",
    type: "new",
    status: "bug",
    author: "Jennifer",
  },
  {
    name: "Rewrite API endpoint for improved security",
    type: "new",
    status: "feature",
    author: "Robert",
  },
  {
    name: "Resolve compatibility issue with IE11",
    type: "done",
    status: "bug",
    author: "Karen",
  },
  {
    name: "Create automated test suite for checkout process",
    type: "new",
    status: "feature",
    author: "Chris",
  },
  {
    name: "Improve accessibility for users with visual impairments",
    type: "new",
    status: "feature",
    author: "Amanda",
  },
  {
    name: "Fix broken link in footer",
    type: "done",
    status: "bug",
    author: "Tom",
  },
];
</script>

<style scoped></style>
