<template>
  <div class="container">
    <h6 class="title">Sprint Name</h6>
    <div class="q-pa-md row items-start q-gutter-md">
      <q-card class="card text-white" style="background-color: #1976d2">
        <q-card-section>
          <div class="text-h6">Date created</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none">{{ dateCreated }}</q-card-section>
      </q-card>

      <q-card class="card text-white" style="background-color: #1976d2">
        <q-card-section>
          <div class="text-h6">Start date</div>
        </q-card-section>

        <q-separator class="card-separator" inset />

        <q-card-section class="q-pt-none"> {{ dateStart }}</q-card-section>
      </q-card>

      <q-card class="card text-white" style="background-color: #1976d2">
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
      {{ description }}
    </p>
  </div>
</template>

<script setup>
import { ref } from "vue";

const description = ref(
  "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Adipisci consequatur voluptatem recusandae suscipit, fugit blanditiis aut,accusamus facilis neque cupiditate similique! Maiores, totam natus exmolestiae repellendus deleniti eum delectus."
);
const dateCreated = ref("20.12.2023");
const dateStart = ref("20.12.2023");
const dateEnd = ref("20.12.2023");
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
</style>
