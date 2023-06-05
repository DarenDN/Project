<template>
  <div class="q-pa-md">
    <q-table
      flat
      bordered
      title="Sprints"
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
import { store } from "stores/store";

const props = defineProps({
  records: {
    type: Array,
    default: () => [],
  },
});
const columns = [
  {
    name: "name",
    required: true,
    label: "Name",
    align: "left",
    field: (row) => row.name,
    format: (val) => `${val}`,
    sortable: true,
  },
  { name: "type", label: "Type", field: "type", sortable: true },
  { name: "status", label: "Status", field: "status", sortable: true },
  { name: "author", label: "Author", field: "author", sortable: true },
];
</script>

<style scoped>

</style>
