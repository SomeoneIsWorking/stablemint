import { defineStore } from "pinia";
import { ref, computed } from "vue";

type User = {
  email: string;
};

export const useUserStore = defineStore("user", () => {
  const user = ref<User | null>(null);

  const isLoggedIn = computed(() => !!user.value);

  function setUser(userData: User | null) {
    user.value = userData;
  }

  return {
    user,
    isLoggedIn,
    setUser,
  };
});
