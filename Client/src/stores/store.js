import { reactive } from "vue";

export const store = reactive({
  afterAuthentication: false,
  newAccountHasBeenRegistered: false,
  drawer: null,
  adminPanel: null,
  sprintPanel: null,
  setDrawer(drawer) {
    this.drawer = drawer;
  },
  setAdminPanel(adminPanel) {
    this.adminPanel = adminPanel;
  },
  setSprintPanel(sprintPanel) {
    this.sprintPanel = sprintPanel;
  },
});
