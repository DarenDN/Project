<template>
  <div class="q-pa-md">
    <q-table
      flat
      bordered
      title="Tasks"
      :rows="records"
      :columns="columns"
      row-key="name"
    >
      <template v-slot:header="props">
        <q-tr :props="props">
          <q-th auto-width />
          <q-th v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.label }}
          </q-th>
        </q-tr>
      </template>

      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td auto-width>
            <q-btn
              size="sm"
              color="primary"
              round
              dense
              @click="() => true"
              icon="open_in_new"
            />
          </q-td>
          <q-td v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.value }}
          </q-td>
        </q-tr>
      </template>
    </q-table>
  </div>
</template>

<script setup>
import { defineProps } from "vue";
import { ref, onMounted } from "vue";
import { httpClient } from "src/utils/httpClient";
import { mask } from "src/utils/mask";

const usersMapping = ref(new Map());

mask.show();
const props = defineProps({
  source: {
    type: String
  },
});

onMounted(function () {
  httpClient
    .get(httpClient.ProjectManagementServicePath, props.source)
    .then((response) => response.json())

    .then((response) => {
      records.value = response.tasks;

      const users = records.value.map((task) => task.creator).flat();
      return httpClient.put(
        httpClient.IdentityManagementPath,
        "IdentityManagement/GetShortUserInfosAsync",
        users
      );
    })
    .then((resp) => resp.json())
    .then((result) => {
      if (result) {
        result.forEach((user) => {
          usersMapping.value.set(user.id, `${user.firstName} ${user.lastName}`);
        });
      }

      records.value.forEach((x) => {
        x.creatorName = usersMapping.value.get(x.creator);
      });
      mask.hide();
    });
});

const records = ref([]);

const columns = [
  {
    name: "title",
    required: true,
    label: "Name",
    align: "left",
    field: "title",
    format: (val) => `${val}`,
    sortable: true,
  },
  { name: "type", label: "Type", field: "type", sortable: true },
  { name: "status", label: "Status", field: "status", sortable: true },
  { name: "creator", label: "Creator", field: "creatorName", sortable: true },
];
</script>

<style scoped></style>
