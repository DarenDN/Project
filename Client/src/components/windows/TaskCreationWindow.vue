<template>
  <q-dialog>
    <q-card>
      <q-card-section>
        <div class="text-h6">Create new task</div>
      </q-card-section>

      <q-separator />

      <q-card-section
        style="max-height: 90vh; width: 550px; padding: 20px 20px 0px 20px"
        class="q-pa-md scroll"
      >
        <q-form @submit="onSubmit" class="q-gutter-md form">
          <div class="form-section">
            <q-input
              filled
              v-model="taskName"
              label="Task name"
              lazy-rules
              :rules="[(val) => val !== '' && val !== null]"
            />
          </div>

          <div class="form-section">
            <q-select
              filled
              v-model="currentTaskType"
              :options="taskTypes"
              label="Task type"
            />
          </div>

          <div class="form-section">
            <q-checkbox
              v-model="addToCurrentSprint"
              label="Add to current sprint"
            />
          </div>

          <div class="form-section">
            <q-input
              v-model="discription"
              filled
              type="textarea"
              label="Discription"
            />
          </div>

          <q-separator />

          <div style="margin: 5px">
            <q-card-actions align="right">
              <q-btn flat label="Submit" color="primary" type="submit" />
            </q-card-actions>
          </div>
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script setup>
import { ref } from "vue";
import { store } from "stores/store";

const taskTypes = ["bug", "feature"];
const currentTaskType = ref(taskTypes[0]);
const addToCurrentSprint = ref(false);
const taskName = ref(null);
const discription = ref(null);

function onSubmit() {
  // TODO: update the store
  mask.show("Loading");

  setTimeout(() => {
    mask.hide();
  }, 1000);
}
</script>

<style scoped>
.form-section {
  margin: 5px;
}
</style>
