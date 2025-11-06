<template>
  <div class="login-page">
    <div class="login-card">
      <h2>Login</h2>

      <form @submit.prevent="handleLogin">
        <input v-model="email" placeholder="Email" type="email" required />
        <input v-model="password" placeholder="Password" type="password" required />

        <button type="submit">Login</button>
      </form>

    
      <p v-if="successMessage" class="success-msg">{{ successMessage }}</p>
      
      <p v-if="errorMessage" class="error-msg">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";

const email = ref("");
const password = ref("");
const successMessage = ref("");
const errorMessage = ref("");

const router = useRouter();
const auth = useAuthStore();

const handleLogin = async () => {
  successMessage.value = "";
  errorMessage.value = "";

  try {
    await auth.login({ email: email.value, password: password.value });
    successMessage.value = "Login successful";
    setTimeout(() => {
      router.push("/blogs");
    }, 1000); 
  } catch (err) {
    console.log(err.response.data);
    if (err.response?.data?.error) {
      errorMessage.value = err.response.data.error;
    } 
  }
};
</script>

<style scoped>
.login-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f4f6f8;
}

.login-card {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
  width: 350px;
  text-align: center;
}

h2 {
  margin-bottom: 1.5rem;
  color: #2c3e50;
}

form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

input {
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
}

input:focus {
  border-color: #2c3e50;
  outline: none;
}

button {
  background-color: #2c3e50;
  color: white;
  padding: 0.75rem;
  border: none;
  border-radius: 6px;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.3s;
}

button:hover {
  background-color: #2980b9;
}

.success-msg {
  margin-top: 1rem;
  color: #27ae60;
  font-weight: 500;
}

.error-msg {
  margin-top: 1rem;
  color: #e74c3c;
  font-weight: 500;
}
</style>
