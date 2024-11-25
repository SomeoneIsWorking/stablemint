import { createRouter, createWebHistory } from "vue-router";
import { useUserStore } from "./stores/user";

const Home = () => import("./components/Home.vue");
const Login = () => import("./components/Login.vue");

const routes = [
  { path: "/", component: Home },
  { path: "/login", component: Login },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, _from, next) => {
  const userStore = useUserStore();
  if (to.path !== "/login" && !userStore.isLoggedIn) {
    next("/login");
  } else {
    next();
  }
});

export default router;
