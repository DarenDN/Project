<template>
    <div class="container">
      <h5 class="title">{{ store?.project?.title }}</h5>
      <div class="q-pa-md row items-start q-gutter-md">
        <q-card class="card" bordered>
          <q-card-section>
            <div class="text-h6">Date created</div>
          </q-card-section>
  
          <q-separator class="card-separator" inset />
  
          <q-card-section class="q-pt-none">{{ dateCreated }}</q-card-section>
        </q-card>
      </div>
  
      <div class="separator">
        <q-separator inset />
      </div>
      <h6 class="title">Description</h6>
      <div class="description">
        {{ store?.project?.description }}
      </div>
    </div>
  </template>
  
  <script setup>
  import { ref } from "vue";
  import { store } from "stores/store";
  const dateCreated = ref(store?.project?.dateCreated);
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
  