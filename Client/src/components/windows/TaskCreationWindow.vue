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
              :options="taskTypesToDispay"
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
              v-model="description"
              filled
              type="textarea"
              label="description"
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
import { ref, onMounted, defineProps } from "vue";
import { store } from "stores/store";
import { mask } from "src/utils/mask";
import { httpClient } from "src/utils/httpClient";
import { useQuasar } from "quasar";

const q = useQuasar();

const taskTypes = ref([]);
const taskTypesToDispay = ref([]);
const currentTaskType = ref(null);
const addToCurrentSprint = ref(false);
const taskName = ref(null);
const description = ref(null);

onMounted(function () {
  httpClient
    .get(httpClient.ProjectManagementServicePath, "Type/GetTypesAsync")
    .then((response) => response.json())
    .then((result) => {
      taskTypes.value = result.types;
      taskTypesToDispay.value = result.types.map((x) => x.name);
    });
});

function onSubmit() {
  // TODO: update the store
  mask.show("Loading");
  const createTaskDto = {
    title: taskName.value,
    description: description.value,
    typeId: taskTypes.value.filter((x) => x.name === currentTaskType.value)[0]
      .id,
    sprintId: addToCurrentSprint.value ? store.sprint.id : null,
  };

  httpClient
    .post(
      httpClient.ProjectManagementServicePath,
      "Task/CreateTaskAsync",
      createTaskDto
    )
    .then((response) => {
      if (!response.ok) {
        q.notify({
          message: "Something went wrong",
          color: "primary",
        });
      }
      
      mask.hide();
    });

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
