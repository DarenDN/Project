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
      <q-card
        class="popup user-popup bg-primary text-white"
        v-morph:userpopup:boxes:600.tween="morphGroupModel"
      >
        <q-card-section>
          <div class="text-h6">Profile</div>
          <div class="text-subtitle2">
            Hi, here you can edit your profile or switch it
          </div>
        </q-card-section>

        <q-separator dark />

        <q-card-actions>
          <q-btn
            class="popup-button"
            color="blue"
            label="Next"
            @click="nextMorph"
          />
          <q-btn
            class="popup-button"
            color="secondary"
            label="Skip"
            @click="finishPopup"
          />
        </q-card-actions>
      </q-card>

      <q-card
        class="popup sprint-popup bg-primary text-white"
        v-morph:sprintpopup:boxes:600.tween="morphGroupModel"
      >
        <q-card-section>
          <div class="text-h6">Sprint</div>
          <div class="text-subtitle2">
            Here you can check the latest information about current spring or
            see actual task on the board
          </div>
        </q-card-section>

        <q-separator dark />

        <q-card-actions>
          <q-btn
            class="popup-button"
            color="blue"
            label="Next"
            @click="nextMorph"
          />
          <q-btn
            class="popup-button"
            color="secondary"
            label="Skip"
            @click="finishPopup"
          />
        </q-card-actions>
      </q-card>

      <q-card
        class="popup project-popup bg-primary text-white"
        v-morph:projectpopup:boxes:600.tween="morphGroupModel"
      >
        <q-card-section>
          <div class="text-h6">Project</div>
          <div class="text-subtitle2">
            Here you can check the latest information about you project
          </div>
        </q-card-section>

        <q-separator dark />

        <q-card-actions>
          <q-btn
            class="popup-button"
            color="blue"
            label="Next"
            @click="nextMorph"
          />
          <q-btn
            class="popup-button"
            color="secondary"
            label="Skip"
            @click="finishPopup"
          />
        </q-card-actions>
      </q-card>

      <q-card
        class="popup administration-popup bg-primary text-white"
        v-morph:administrationpopup:boxes:600.tween="morphGroupModel"
      >
        <q-card-section>
          <div class="text-h6">Administration</div>
          <div class="text-subtitle2">
            Hey, that's a secret place
          </div>
        </q-card-section>

        <q-separator dark />

        <q-card-actions>
          <q-btn color="blue" label="Got it" @click="finishPopup" />
        </q-card-actions>
      </q-card>
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
      store.projectPanel.show();
      morphGroupModel.value = "projectpopup";
      break;
    }
    case "projectpopup": {
      if (store.user.isAdmin) {
        store.adminPanel.show();
        morphGroupModel.value = "administrationpopup";
      } else {
        finishPopup();
      }
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

setTimeout(() => {
  if (store.newAccountHasBeenRegistered) {
    store.newAccountHasBeenRegistered = false; // to make sure that user wont see it again after relogin
    startPopup();
  }
}, 500);
</script>

<style scoped>
.popup {
  position: absolute;
}
.user-popup {
  position: absolute;
  top: 10px;
  right: 30px;
}
.sprint-popup {
  top: 20px;
  left: 10px;
  width: 300px;
  height: 200px;
}
.project-popup {
  top: 200px;
  left: 10px;
  width: 300px;
  height: 200px;
}
.administration-popup {
  top: 350px;
  left: 10px;
}
.popup-button {
  margin: 0px 10px;
}
img {
  opacity: 0.5;
}
</style>
