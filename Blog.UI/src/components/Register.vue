<template>
  <div class="register-page">
    <div class="register-card">
      <h2>Register</h2>

      <form @submit.prevent="handleRegister">
        <input v-model="name" placeholder="Name" required />
        <input v-model="email" type="email" placeholder="Email" required />
        <input v-model="password" type="password" placeholder="Password" required />

       
        <select  v-model="role" required >
          <option disabled value="">Select Role</option>
          <option value="Maker">Maker</option>
          <option value="Checker">Checker</option>
        </select>

        <button type="submit">Register</button>
      </form>

      <p v-if="successMessage" class="success-msg">{{ successMessage }}</p>
      <p v-if="errorMessage" class="error-msg">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useAuthStore } from "../stores/auth.js";

const name = ref("");
const email = ref("");
const password = ref("");
const role = ref(""); 
const successMessage = ref("");
const errorMessage = ref("");

const auth = useAuthStore();

const handleRegister = async () => {
  successMessage.value = "";
  errorMessage.value = "";

  try {
    await auth.register({
      name: name.value,
      email: email.value,
      password: password.value,
      role: role.value, 
    });
    successMessage.value = "Registered successfully";
    name.value = "";
    email.value = "";
    password.value = "";
    role.value = "";
  } catch (err) {
    if (err.response?.data?.message) {
      errorMessage.value = err.response.data.message;
    } else {
      errorMessage.value = "Registration failed. Please try again.";
    }
  }
};
</script>

<style scoped>
.register-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: 100vh;
  background: #f4f6f8;
}

.register-card {
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

input,
select {
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 6px;
  font-size: 1rem;
}

input:focus,
select:focus {
  border-color: #3498db;
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
