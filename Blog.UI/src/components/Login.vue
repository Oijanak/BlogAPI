<template>
  <div class="login-page">
    <div class="login-card">
      <h2>Login</h2>

      <!-- Email / Password Form -->
      <form @submit.prevent="handleLogin">
        <input
          v-model.trim="email"
          placeholder="Email"
          type="email"
          required
        />
        <input
          v-model.trim="password"
          placeholder="Password"
          type="password"
          required
        />

        <button type="submit">Login</button>
      </form>

      <div class="divider">or</div>

      <!-- Google Sign-In -->
      <div id="g_id_signin"></div>

      <p v-if="successMessage" class="success-msg">{{ successMessage }}</p>
      <p v-if="errorMessage" class="error-msg">{{ errorMessage }}</p>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";

const email = ref("");
const password = ref("");
const successMessage = ref("");
const errorMessage = ref("");

const router = useRouter();
const auth = useAuthStore();

// ✅ Your Google Client ID
const GOOGLE_CLIENT_ID =
  "855256408648-rnc3jaa37m47inn9scmdfaa7e18pjb8u.apps.googleusercontent.com";

onMounted(() => {
  // Ensure Google SDK is loaded
  if (!window.google || !window.google.accounts) {
    console.error("Google Identity script not loaded");
    return;
  }

  // Initialize Google One Tap / Sign-In button
  window.google.accounts.id.initialize({
    client_id: GOOGLE_CLIENT_ID,
    callback: handleCredentialResponse,
  });

  // Render the Google Sign-In button
  window.google.accounts.id.renderButton(
    document.getElementById("g_id_signin"),
    { theme: "outline", size: "large", width: "300" }
  );
});

// ✅ Google login callback
async function handleCredentialResponse(response) {
  const idToken = response.credential;
  console.log("Google ID Token:", idToken);

  try {
    // Send the ID token to backend for validation
    const res = await auth.loginWithGoogle({ idToken });
    successMessage.value = "Login successful";

    setTimeout(() => { router.push("/blogs"); }, 1000);
  } catch (err) {
    console.error("Google login failed:", err);
    errorMessage.value = "Google login failed. Please try again.";
  }
}

// ✅ Email-password login
const handleLogin = async () => {
  successMessage.value = "";
  errorMessage.value = "";

  try {
    await auth.login({ email: email.value, password: password.value });
    successMessage.value = "Login successful";

    setTimeout(() => { router.push("/blogs"); }, 1000);
  } catch (err) {
    console.error("Login error:", err);
    errorMessage.value = err.response?.data?.error || "Login failed";
  }
};
</script>

<style scoped>
/* Layout */
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

/* Form */
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

/* Divider */
.divider {
  margin: 1.5rem 0;
  position: relative;
  color: #888;
  font-size: 0.9rem;
}

.divider::before,
.divider::after {
  content: "";
  position: absolute;
  top: 50%;
  width: 40%;
  height: 1px;
  background: #ddd;
}

.divider::before {
  left: 0;
}

.divider::after {
  right: 0;
}

/* Messages */
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
