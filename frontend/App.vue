<template>
  <div id="app">
    <header class="app-header">
      <div class="nav-container">
        <div v-if="userStore.user" class="user-section">
          <span>Welcome {{ userStore.user.email }}</span>
          <button class="logout-btn" @click="logout">Logout</button>
        </div>
        <div v-else>
          <router-link class="login-link" to="/login">Login</router-link>
        </div>
      </div>
    </header>
    <router-view></router-view>
  </div>
</template>

<script setup lang="ts">
import { useUserStore } from "./stores/user";
import { useRouter } from "vue-router";
import axios from "axios";

const router = useRouter();
const userStore = useUserStore();

const logout = async () => {
  await axios.post("/api/auth/logout");
  userStore.setUser(null);
  router.push("/login");
};
</script>

<style scoped>
.app-header {
  background-color: #f8f9fa;
  padding: 1rem;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.nav-container {
  max-width: 1200px;
  margin: 0 auto;
  display: flex;
  justify-content: flex-end;
}

.user-section {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.logout-btn {
  padding: 0.5rem 1rem;
  background-color: #dc3545;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.logout-btn:hover {
  background-color: #c82333;
}

.login-link {
  color: #007bff;
  text-decoration: none;
  font-weight: 500;
}

.login-link:hover {
  text-decoration: underline;
}
</style>
