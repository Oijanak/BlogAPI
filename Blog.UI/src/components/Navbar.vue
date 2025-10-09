<script setup>
import { useAuthStore } from "../stores/auth";
import { computed } from "vue";
import { useRouter } from "vue-router";

const authStore = useAuthStore();
const router = useRouter();

const isLoggedIn = computed(() => authStore.isAuthenticated);
const user = computed(() => authStore.currentUser);

function handleLogout() {
  authStore.logout();
  router.push("/login");
}
</script>

<template>
  <nav class="navbar">
    <router-link v-if="!isLoggedIn" to="/register">Register</router-link>
    <router-link v-if="!isLoggedIn" to="/login">Login</router-link>
    
    <div v-if="isLoggedIn" class="user-info">
      {{ user?.email +" || "+user?.role }}
    </div>

    <button v-if="isLoggedIn" @click="handleLogout" class="logout-btn">
      Logout
    </button>
  </nav>
</template>

<style scoped>
.navbar {
  background-color: #2c3e50;
  padding: 1rem;
  display: flex;
  gap: 1rem;
  align-items: center;
}

.navbar a {
  color: #ecf0f1;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.3s;
}

.navbar a:hover {
  color: #1abc9c;
}

.user-info {
  color: #ecf0f1;
  font-weight: 600;
  background-color: rgba(255, 255, 255, 0.1);
  padding: 0.3rem 0.6rem;
  border-radius: 6px;
  font-size: 0.9rem;
}

.logout-btn {
  background-color: #e74c3c;
  color: white;
  border: none;
  padding: 0.4rem 0.8rem;
  border-radius: 4px;
  cursor: pointer;
  font-weight: 500;
  transition: background-color 0.3s;
}

.logout-btn:hover {
  background-color: #c0392b;
}
</style>
