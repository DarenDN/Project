<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />

        <q-toolbar-title>ManageIt</q-toolbar-title>
        <q-breadcrumbs class="text-white" active-color="white">
          <template v-slot:separator>
            <q-icon size="1.2em" name="arrow_forward" color="purple" />
          </template>
          <q-breadcrumbs-el label="Home" icon="home" to="/home" />
          <q-breadcrumbs-el
            label="Components"
            icon="dashboard"
            to="/components"
          />
          <q-breadcrumbs-el
            label="Random task #3228"
            icon="navigation"
            to="/aTask"
          />
        </q-breadcrumbs>
        <q-space />
        <q-space />
        <q-space />
        <q-space />
        <q-space />
        <q-btn color="blue" label="New meeting"> </q-btn>
        <div class="q-pa-md">
          <q-btn color="secondary" label="Account Settings">
            <q-menu>
              <div class="row no-wrap q-pa-md">
                <div class="column">
                  <div class="text-h6 q-mb-md">Settings</div>
                  <q-toggle v-model="checkbox1" label="Checkbox 1" />
                  <q-toggle v-model="checkbox2" label="Checkbox 2" />
                </div>

                <q-separator vertical inset class="q-mx-lg" />

                <div class="column items-center">
                  <q-avatar size="72px">
                    <q-avatar
                      color="primary"
                      text-color="white"
                      icon="person"
                    />
                  </q-avatar>

                  <div class="text-subtitle1 q-mt-md q-mb-xs">
                    Billy Herrington
                  </div>

                  <q-btn
                    @click="logout()"
                    color="primary"
                    label="Logout"
                    push
                    size="sm"
                    v-close-popup
                  />
                </div>
              </div>
            </q-menu>
          </q-btn>
        </div>
      </q-toolbar>
    </q-header>

    <q-drawer ref="mainDrawer" v-model="leftDrawerOpen" bordered>
      <div class="q-pa-md" style="max-width: 350px">
        <h6 class="project-name"> {{ store?.project?.projectName }}</h6>
        <q-list bordered class="rounded-borders">
          <q-expansion-item
            switch-toggle-side
            expand-separator
            icon="published_with_changes"
            label="Current sprint"
            ref="sprintPanel"
          >
            <q-card>
              <q-card-section>
                <q-btn
                  align="left"
                  class="menu-item"
                  :ripple="false"
                  flat
                  color="white"
                  text-color="black"
                  icon="info"
                  label="Sprint info"
                  to="/home/sprint-info"
                />
                <q-btn
                  align="left"
                  class="menu-item"
                  :ripple="false"
                  flat
                  color="white"
                  text-color="black"
                  icon="dashboard"
                  label="Dashboard"
                  to="/home/dashboard"
                />
                <q-btn
                  align="left"
                  class="menu-item"
                  :ripple="false"
                  flat
                  color="white"
                  text-color="black"
                  icon="checklist_rtl"
                  label="Sprint backlog"
                  to="/home/sprint-backlog"
                />
              </q-card-section>
            </q-card>
          </q-expansion-item>

          <q-expansion-item
            switch-toggle-side
            expand-separator
            icon="event_available"
            label="Project"
            ref="projectPanel"
          >
            <q-card>
              <q-card-section>
                <q-btn
                  align="left"
                  class="menu-item"
                  :ripple="false"
                  flat
                  color="white"
                  text-color="black"
                  icon="info"
                  label="Project info"
                  to="/home/project-info"
                />
                <q-btn
                  align="left"
                  class="menu-item"
                  :ripple="false"
                  flat
                  color="white"
                  text-color="black"
                  icon="checklist_rtl"
                  label="Project backlog"
                  to="/home/project-backlog"
                />
              </q-card-section>
            </q-card>
          </q-expansion-item>

          <q-expansion-item
            switch-toggle-side
            expand-separator
            icon="manage_accounts"
            label="Admin board"
            ref="adminPanel"
          >
            <q-card>
              <q-card-section>
                <q-btn
                  align="left"
                  class="menu-item"
                  :ripple="false"
                  flat
                  color="white"
                  text-color="black"
                  icon="person"
                  label="Users"
                  to="/home/users"
                />
              </q-card-section>
            </q-card>
          </q-expansion-item>
        </q-list>
      </div>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { store } from "stores/store";
import { useQuasar } from "quasar";
import { mask } from "src/utils/mask";

const mainDrawer = ref(null);
const adminPanel = ref(null);
const sprintPanel = ref(null);
const projectPanel = ref(null);
const leftDrawerOpen = ref(false);
const checkbox1 = ref(false);
const checkbox2 = ref(false);
const router = useRouter();

const q = useQuasar();

mask.init(q.loading);

onMounted(() => {
  store.drawer = mainDrawer.value;
  store.adminPanel = adminPanel.value;
  store.sprintPanel = sprintPanel.value;
  store.projectPanel = projectPanel.value;
});

function toggleLeftDrawer() {
  leftDrawerOpen.value = !leftDrawerOpen.value;
}

function logout() {
  router.push("/login");
}
</script>

<style>
.menu-item {
  width: 100%;
}
.project-name {
  margin: 10px;
  padding: 0;
}
</style>
