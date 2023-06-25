<template>
  <q-dialog>
    <q-card>
      <q-card-section>
        <div class="text-h6">{{ title }}</div>
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
              v-model="votedEstimateValue"
              mask="fulltime"
              :rules="['fulltime']"
              label="Estimate"
            >
              <template v-slot:append>
                <q-icon name="access_time" class="cursor-pointer">
                  <q-popup-proxy
                    cover
                    transition-show="scale"
                    transition-hide="scale"
                  >
                    <q-time v-model="votedEstimateValue" with-seconds format24h>
                      <div class="row items-center justify-end">
                        <q-btn
                          v-close-popup
                          label="Close"
                          color="primary"
                          flat
                        />
                      </div>
                    </q-time>
                  </q-popup-proxy>
                </q-icon>
              </template>
            </q-input>
          </div>

          <div class="form-section">
            <q-select
              filled
              v-model="votedPointValue"
              label="Difficulty point"
              :options="points"
              behavior="menu"
            />
          </div>
          <q-separator />

          <div style="margin: 5px">
            <q-card-actions align="right">
              <q-btn flat label="Reset" color="red" type="reset" />
              <q-btn
                label="Submit"
                color="primary"
                type="submit"
                v-close-popup
              />
            </q-card-actions>
          </div>
        </q-form>
      </q-card-section>
    </q-card>
  </q-dialog>
</template>

<script setup>
import { ref, defineProps } from "vue";
import { store } from "stores/store";
import { mask } from "src/utils/mask";
import { httpClient } from "src/utils/httpClient";

const props = defineProps({
  title: {
    type: String,
  },
  currentTask: {
    type: Object,
  },
  endpont: {
    type: String
  },
  callback: {
    type: Function
  }
});

const points = [1, 2, 3, 5, 8, 13, 21];
const votedPointValue = ref(points[0]);
const votedEstimateValue = ref(0);

function onSubmit() {
  mask.show();
  const data = {
    taskId: props.currentTask.id,
    evaluationPoints: votedPointValue.value,
    evaluationTime: votedEstimateValue.value,
  };
  httpClient
    .post(
      httpClient.MeetingServicePath,
      props.endpont,
      data
    )
    .then((response) => {
      mask.hide();
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
