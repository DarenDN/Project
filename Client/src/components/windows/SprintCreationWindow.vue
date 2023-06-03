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
            <q-input v-model="currentTaskType" filled label="Add task type">
              <template v-slot:append>
                <q-btn round dense flat icon="add" @click="addTaskType()" />
              </template>
            </q-input>

            <div class="form-section task-types">
              <q-badge
                v-for="taskType in taskTypes"
                :key="taskType"
                outline
                color="primary"
                :label="taskType"
              />
            </div>
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
              <q-btn flat label="Create" color="primary" type="submit" />
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

const router = useRouter();

const taskTypes = ref(["new", "done"]);
const sprintName = ref(null);
const startDate = ref(null);
const endDate = ref(null);
const discription = ref(null);
const currentTaskType = ref(null);

function addTaskType() {
  if (currentTaskType.value == null || currentTaskType.value.length === 0)
    return;

  taskTypes.value.push(currentTaskType.value);
  currentTaskType.value = null;
}

function onSubmit() {
  // TODO: update the store
  mask.show("Loading");

  setTimeout(() => {
    store.sprint = {
      sprintName: sprintName.value,
      startDate: startDate.value,
      endDate: endDate.value,
      dateCreated: "2000/01/01",
      discription,
      taskTypes: taskTypes.value
    };
    router.push("/home/sprint-info");
    mask.hide();
  }, 1000);
}
</script>

<style scoped>
.form-section {
  margin: 5px;
}
.task-types > div {
  margin-left: 8px;
  margin-bottom: 3px;
}
</style>
