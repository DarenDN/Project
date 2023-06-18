<template>
  <q-dialog>
    <q-card>
      <q-card-section>
        <div class="text-h6">Create new sprint</div>
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
              v-model="sprintName"
              label="Sprint name"
              lazy-rules
              :rules="[(val) => val !== '' && val !== null]"
            />
          </div>

          <div class="form-section">
            <q-input
              filled
              v-model="startDate"
              mask="date"
              label="Start date"
              :rules="['date']"
            >
              <template v-slot:append>
                <q-icon name="event" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-date v-model="startDate">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-date>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>

          <div class="form-section">
            <q-input
              filled
              v-model="endDate"
              mask="date"
              label="End date"
              :rules="['date']"
            >
              <template v-slot:append>
                <q-icon name="event" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-date v-model="endDate">
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-date>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>

          <div class="form-section">
            <q-input v-model="currentTaskStatus" filled label="Add task status">
              <template v-slot:append>
                <q-btn round dense flat icon="add" @click="addTaskStatus()" />
              </template>
            </q-input>

            <div class="form-section task-statuses">
              <q-badge
                v-for="taskStatus in taskStatuses"
                :key="taskStatus"
                outline
                color="primary"
                :label="taskStatus"
              />
            </div>
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
import { ref } from "vue";
import { store } from "stores/store";
import { mask } from "src/utils/mask";
import { useRouter } from "vue-router";
import { useQuasar } from "quasar";
import { httpClient } from "src/utils/httpClient";

const router = useRouter();
const q = useQuasar();

const taskStatuses = ref(["new", "done"]);
const sprintName = ref(store?.sprint?.sprintName);
const startDate = ref(store?.sprint?.startDate);
const endDate = ref(store?.sprint?.endDate);
const description = ref(
  store?.sprint?.description || "Type sprint description here"
);
const currentTaskStatus = ref(null);

function addTaskStatus() {
  if (currentTaskStatus.value == null || currentTaskStatus.value.length === 0)
    return;

  taskStatuses.value.push(currentTaskStatus.value);
  currentTaskStatus.value = null;
}

function onSubmit() {
  // TODO: update the store
  mask.show("Loading");

  const sprintData = {
    name: sprintName.value,
    description: description.value,
    dateStart: new Date(startDate.value),
    dateEnd: new Date(endDate.value),
  };

  httpClient
    .post(
      httpClient.ProjectManagementServicePath,
      "Sprint/CreateSprintAsync",
      sprintData
    )
    .then((response) => {
      if (response.ok) {
        router.push("/home/sprint-info");
        mask.hide();
        store.sprint = {
          sprintName: sprintName.value,
          startDate: startDate.value,
          endDate: endDate.value,
          dateCreated: "2000/01/01",
          description,
          taskStatuses: taskStatuses.value,
        };
      } else {
        q.notify({
          message: 'Something went wrong',
          color: "primary",
        });
        mask.hide();
      }
    });
}
</script>

<style scoped>
.form-section {
  margin: 5px;
}
.task-statuses > div {
  margin-left: 8px;
  margin-bottom: 3px;
}
</style>
