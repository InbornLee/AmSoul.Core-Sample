<template>
  <div class="container">
    <h1 id="tableLabel">天气</h1>
    <p>This component demonstrates fetching data from the server.</p>
    <p v-if="!forecasts"><em>Loading...</em></p>
    <table class="table table-striped" aria-labelledby="tableLabel" v-if="forecasts">
      <thead>
        <tr>
          <th>Date</th>
          <th>Temp. (C)</th>
          <th>Temp. (F)</th>
          <th>Summary</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="forecast of forecasts" v-bind:key="forecast">
          <td>{{ forecast.date }}</td>
          <td>{{ forecast.temperatureC }}</td>
          <td>{{ forecast.temperatureF }}</td>
          <td>{{ forecast.summary }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>


<script setup>
import axios from "axios";
import { onMounted, ref } from "vue";
const forecasts = ref([]);
const getWeatherForecasts = () => {
  axios
    .get("/weatherforecast")
    .then((response) => {
      forecasts.value = response.data;
    })
    .catch(function (error) {
      alert(error);
    });
};
onMounted(() => {
  getWeatherForecasts();
});
</script>