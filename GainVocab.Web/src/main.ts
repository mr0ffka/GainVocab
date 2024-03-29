import "element-plus/dist/index.css";
import "element-plus/theme-chalk/dark/css-vars.css";
import "./assets/style.css";

import App from "./App.vue";
import router from "./router";
import { createApp } from "vue";
import ElementPlus from "element-plus";
import VueAxios from "vue-axios";
import axios from "axios";
import { VueQueryPlugin } from "@tanstack/vue-query";
import { queryClient } from "@/helpers/queryClient";
import { createPinia } from "pinia";

createApp(App)
  .use(router)
  .use(createPinia())
  .use(ElementPlus, { zIndex: 3000 })
  .use(VueQueryPlugin, { queryClient })
  .use(VueAxios, axios)
  .mount("#app");
