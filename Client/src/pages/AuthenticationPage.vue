<template>
  <q-slide-transition>
    <q-form
      @submit="onSubmit"
      @reset="onReset"
      class="q-gutter-md form"
      v-show="!registration"
      style="width: 500px"
    >
      <q-input
        filled
        v-model="authorizationLogin"
        label="Your login"
        lazy-rules
        :rules="[(val) => checkNameOrLogin(val)]"
      />

      <q-input
        filled
        type="password"
        v-model="authorizationPassword"
        label="Your password"
        lazy-rules
        :rules="[(val) => checkPassword(val)]"
      />

      <div>
        <q-btn
          label="Submit"
          type="submit"
          color="primary"
        />
        <q-btn
          label="I dont't have an account"
          type="reset"
          color="primary"
          flat
          class="q-ml-sm"
        />
      </div>
    </q-form>
  </q-slide-transition>

  <q-slide-transition>
    <q-form
      @submit="onSubmit"
      @reset="onReset"
      class="q-gutter-md form"
      v-show="registration"
      style="width: 500px"
    >
      <q-stepper v-model="step" vertical color="primary" animated>
        <q-step
          :name="1"
          title="Account information"
          icon="settings"
          :done="step > 1"
          :error="checkNameOrLogin(registrationLogin) !== true || checkPassword(registrationPassword) !== true || checkPassword(registrationPasswordAgain) !== true || checkPasswords(registrationPassword, registrationPasswordAgain) !== true"
        >
          <q-input
            filled
            v-model="registrationLogin"
            label="Your login"
            lazy-rules
            :rules="[(val) => checkNameOrLogin(val)]"
          />

          <q-input
            filled
            type="password"
            v-model="registrationPassword"
            label="Your password"
            lazy-rules
            :rules="[(val) => checkPassword(val)]"
          />

          <q-input
            filled
            type="password"
            v-model="registrationPasswordAgain"
            label="Your password again"
            lazy-rules
            :rules="[(val) => checkPassword(val)]"
          />

          <q-stepper-navigation>
            <q-btn
              @click="step = 2"
              color="primary"
              label="Continue"
            />
            <q-btn
              label="I have an account"
              type="reset"
              color="primary"
              flat
              class="q-ml-sm"
            />
          </q-stepper-navigation>
        </q-step>

        <q-step
          :name="2"
          title="Personal information"
          caption="(Optional)"
          icon="create_new_folder"
          :done="step > 2"
        >
          <q-input
            filled
            v-model="firstName"
            label="First name"
            lazy-rules
            :rules="[(val) => true]"
          />
          <q-input
            filled
            v-model="secondName"
            label="Last name"
            lazy-rules
            :rules="[(val) => true]"
          />
          <q-input
            filled
            v-model="patronymic"
            label="Patronymic"
            lazy-rules
            :rules="[(val) => true]"
          />

          <q-stepper-navigation>
            <q-btn
              @click="step = 4"
              color="primary"
              label="Continue"
            />
            <q-btn
              flat
              @click="step = 1"
              color="primary"
              label="Back"
              class="q-ml-sm"
            />
          </q-stepper-navigation>
        </q-step>

        <q-step :name="4" title="Project Information" icon="add_comment">
          <q-input
            filled
            v-model="projectName"
            label="Project name"
            lazy-rules
            :rules="[(val) => checkNameOrLogin(val)]"
          />
          <q-input
            filled
            v-model="projectDescription"
            label="Project description"
            lazy-rules
            :rules="[(val) => checkNameOrLogin(val)]"
          />

          <q-stepper-navigation>
            <q-btn
              color="primary"
              label="Finish"
              type="submit"
            />
            <q-btn
              flat
              @click="step = 2"
              color="primary"
              label="Back"
              class="q-ml-sm"
            />
          </q-stepper-navigation>
        </q-step>
      </q-stepper>
    </q-form>
  </q-slide-transition>
</template>

<script setup>
import { useQuasar } from "quasar";
import { ref } from "vue";
import { useRouter } from "vue-router";
import { store } from "stores/store";

const $q = useQuasar();
const router = useRouter();

const authorizationLogin = ref(null);
const authorizationPassword = ref(null);

const firstName = ref(null);
const secondName = ref(null);
const patronymic = ref(null);

const registrationLogin = ref(null);
const registrationPassword = ref(null);
const registrationPasswordAgain = ref(null);

const projectName = ref(null);
const projectDescription = ref(null);

const registration = ref(false);
const step = ref(1);

function onSubmit() {
  store.newAccountHasBeenRegistered = registration.value;
  store.afterAuthentication = true;
  router.push("/home");
}

function onReset() {
  registration.value = !registration.value;
}

function checkPassword(password) {
  if (!password || password.length < 6) {
    return "Password must contain 6 characters";
  }

  return true;
}

function checkPasswords(firstPassword, secondPassword) {
  var result1 = checkPassword(firstPassword)
  var result2 = checkPassword(secondPassword)

  if (result1 !== true) return result1
  if (result2 !== true) return result2

  if (firstPassword === secondPassword) return "Passwords are not the same"

  return true;
}

function checkNameOrLogin(nameOrLogin) {
  if (!nameOrLogin || nameOrLogin.length < 6) {
    return "Value must contain 6 characters";
  }

  if (/\d/.test(nameOrLogin)) {
    return "Can't contain numbers";
  }

  return true;
}
</script>

<style scoped>
.container {
  width: 100px;
  height: 100px;
}
.form {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
}
</style>
