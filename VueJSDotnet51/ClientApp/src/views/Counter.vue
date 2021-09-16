<template>
  <div class="container">
    <h1>Counter</h1>
    <p>This is a example of an 11 Vue component.</p>
    <p aria-live="polite">
      Current count: <strong>{{ currentCount }}</strong>
    </p>
    <button class="btn btn-primary" @click="incrementCounter">Increment</button>
  </div>
</template>
<script setup>
import { ref } from "vue";
const currentCount = ref(0);
const incrementCounter = () => {
  currentCount.value++;
};
</script>