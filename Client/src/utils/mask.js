import { QSpinnerCube } from "quasar";

export const mask = {
  loading: null,
  show: function (text = "Some important process is in progress. Hang on...") {
    this.loading.show({
      spinner: QSpinnerCube,
      spinnerColor: "blue",
      spinnerSize: 140,
      backgroundColor: "black",
      message: text,
      messageColor: "white",
    });
  },

  hide: function () {
    this.loading.hide();
  },

  init(loading) {
    this.loading = loading;
  }
};
