import { reactive } from "vue";

export const store = reactive({
  newAccountHasBeenRegistered: false,
  sprint: null,
  drawer: null,
  adminPanel: null,
  sprintPanel: null,
  
  reset() {
    this.newAccountHasBeenRegistered = false;
    this.sprint = null;
    this.drawer = null;
    this.adminPanel = null;
    this.sprintPanel = null;
  }
});
