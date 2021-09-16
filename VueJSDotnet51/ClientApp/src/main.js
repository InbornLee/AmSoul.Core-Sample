import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import moment from "moment";
import 'moment/locale/zh-cn';
import "./assets/app.css";

const app = createApp(App);
app.config.globalProperties.$moment = moment;
app.use(store).use(router).mount("#app");
