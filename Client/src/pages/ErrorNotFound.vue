<template>
  <div class="container">
      <div style="font-size: 30vh">
        404
      </div>

      <div class="text-h2" style="opacity:.4">
        Oops. Nothing here...
      </div>

      <q-btn
        class="q-mt-xl"
        color="blue"
        text-color="white"
        unelevated
        to="/home"
        label="Go Home"
        no-caps
      />
    </div>
</template>

<script>
import { defineComponent } from 'vue'

export default defineComponent({
  name: 'ErrorNotFound'
})
</script>

<style>
.container {
  width: 100%;
  text-align: center;
}
</style>
