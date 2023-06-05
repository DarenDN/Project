<template>
  <BacklogView :records="records"></BacklogView>
</template>

<script setup>
import { store } from "stores/store";
import { useRouter } from "vue-router";
import BacklogView from "src/components/BacklogView.vue";

const router = useRouter();

if (!store.sprint) {
  router.push("/home/empty-sprint");
}

const records = [
  {
    name: "Implement login functionality",
    type: "new",
    status: "feature",
    author: "John"
  },
  {
    name: "Fix issue with image uploader",
    type: "done",
    status: "bug",
    author: "Jane"
  },
  {
    name: "Add pagination to search results",
    type: "new",
    status: "feature",
    author: "Bob"
  },
  {
    name: "Resolve conflict with third-party library",
    type: "new",
    status: "bug",
    author: "Alice"
  },
  {
    name: "Design new user interface",
    type: "new",
    status: "feature",
    author: "Mark"
  },
  {
    name: "Update documentation with new features",
    type: "done",
    status: "feature",
    author: "Sarah"
  },
  {
    name: "Fix broken link on homepage",
    type: "done",
    status: "bug",
    author: "Tom"
  },
  {
    name: "Add support for multiple languages",
    type: "new",
    status: "feature",
    author: "Emily"
  },
  {
    name: "Improve performance of search algorithm",
    type: "new",
    status: "feature",
    author: "Alex"
  },
  {
    name: "Investigate issue with server downtime",
    type: "new",
    status: "bug",
    author: "David"
  }
];
</script>

<style></style>
