import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import axios from "axios";
import { createPinia } from "pinia";
import { useUserStore } from "./stores/user";

const app = createApp(App);
const pinia = createPinia();

async function checkAuth() {
  const response = await axios.get("/api/auth");
  if (response.data) {
    const userStore = useUserStore();
    userStore.setUser({ email: response.data });
  } else {
    router.push("/login");
  }
}

async function build() {
  app.use(pinia);
  await checkAuth();
  app.use(router);
  app.mount("#app");
}
build();
