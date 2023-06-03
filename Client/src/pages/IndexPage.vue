<template>
  <q-page class="flex flex-center">
    <q-transition
      appear
      enter-active-class="animated fadeIn"
      leave-active-class="animated fadeOut"
    >
      <img
        alt="logo"
        src="https://pnglib.nyc3.cdn.digitaloceanspaces.com/uploads/2021/02/webinar-computer_602021158036c.png"
      />
    </q-transition>
    <div
      class="q-pa-md absolute-position"
      style="height: 600px; max-height: 80vh"
    >
      <div
        style="position: absolute; top: 50%; left: 50%"
        v-morph:startpopup:boxes:600.tween="morphGroupModel"
      ></div>
      <div
        class="popup user-popup"
        v-morph:userpopup:boxes:600.tween="morphGroupModel"
      >
        <p><b>Hi, here you can edit your profile or switch it</b></p>

        <div class="splitter"></div>
        <div class="button-container">
          <q-btn class="popup-button" color="blue" label="Next" @click="nextMorph" />
          <q-btn class="popup-button" color="secondary" label="Skip" @click="finishPopup" />
        </div>
      </div>

      <div
        class="popup sprint-popup"
        v-morph:sprintpopup:boxes:600.tween="morphGroupModel"
      >
        <p>
          <b
            >Here you can check the latest information about current spring or
            see actual task on the board</b
          >
        </p>

        <div class="splitter"></div>
        <div class="button-container">
          <q-btn class="popup-button" color="blue" label="Next" @click="nextMorph" />
          <q-btn class="popup-button" color="secondary" label="Skip" @click="finishPopup" />
        </div>
      </div>

      <div
        class="popup administration-popup"
        v-morph:administrationpopup:boxes:600.tween="morphGroupModel"
      >
        <p>
          <b>Administration pannel, hey, that's a secret place</b>
        </p>

        <div class="splitter"></div>
        <div>
          <q-btn color="blue" label="Got it" @click="finishPopup" />
        </div>
      </div>
      <div
        style="position: absolute; top: 50%; left: 50%"
        v-morph:finishpopup:boxes:600.tween="morphGroupModel"
      ></div>
    </div>
  </q-page>
</template>

<script setup>
import { ref } from "vue";
import { store } from "stores/store";
import { mask } from "src/utils/mask";

const morphGroupModel = ref("startpopup");
function nextMorph() {
  const currentState = morphGroupModel.value;
  switch (currentState) {
    case "startpopup": {
      morphGroupModel.value = "userpopup";
      break;
    }
    case "userpopup": {
      store.drawer.show();

      setTimeout(() => store.sprintPanel.show(), 600);
      setTimeout(() => (morphGroupModel.value = "sprintpopup"), 200);
      break;
    }
    case "sprintpopup": {
      store.adminPanel.show();
      morphGroupModel.value = "administrationpopup";
      break;
    }
    case "administrationpopup": {
      finishPopup();
      break;
    }
  }
}

function startPopup() {
  morphGroupModel.value = "userpopup";
}

function finishPopup() {
  morphGroupModel.value = "finishpopup";
}

function loadedAnimation() {
  mask.hide();
}

mask.show()

setTimeout(() => {
  loadedAnimation();
  if (store.newAccountHasBeenRegistered) {
    store.newAccountHasBeenRegistered = false // to make sure that user wont see it again after relogin
    startPopup();
  }
}, 2000);
</script>

<style scoped>
.popup {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  position: absolute;
  width: 250px;
  height: 150px;
  border-radius: 25px;
  font-size: 14px;
  background-color: #1976d2;
  color: white;
  text-align: center;
}
.user-popup {
  top: 10px;
  right: 30px;
}
.sprint-popup {
  top: 20px;
  left: 10px;
  width: 300px;
  height: 200px;
}
.administration-popup {
  top: 250px;
  left: 10px;
}
.splitter {
  background-color: white;
  width: 80%;
  height: 1px;
  margin: 5px 0;
}
.button-container {
  display: flex;
  justify-content: space-between;
}
.popup-button {
  margin: 0px 10px;
}
b {
  margin: 5px;
}
img {
  opacity: 0.5;
}
</style>
