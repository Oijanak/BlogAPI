<template>
    <div class="forgot-page">
        <div class="forgot-card">
            <h2>Forgot Password</h2>
            <p class="subtitle">
                Enter your registered email, and we’ll send you a password reset link.
            </p>

            <form @submit.prevent="handleForgotPassword">
                <input v-model.trim="email"
                       type="email"
                       placeholder="Enter your email"
                       required />

                <button type="submit" :disabled="loading">
                    {{ loading ? "Sending..." : "Send Reset Link" }}
                </button>
            </form>

            <p v-if="successMessage" class="success-msg">{{ successMessage }}</p>
            <p v-if="errorMessage" class="error-msg">{{ errorMessage }}</p>

            <router-link to="/login" class="back-link">
                ← Back to Login
            </router-link>
        </div>
    </div>
</template>

<script setup>
import { ref } from "vue";
import { useAuthStore } from "../stores/auth";

const email = ref("");
const loading = ref(false);
const successMessage = ref("");
const errorMessage = ref("");

const auth = useAuthStore();

const handleForgotPassword = async () => {
  successMessage.value = "";
  errorMessage.value = "";
  loading.value = true;

  try {
      await auth.forgotPassword({ email:email.value }); 
    successMessage.value =
      "If this email is registered, a reset link has been sent.";
  } catch (err) {
    console.error("Forgot password error:", err);
    errorMessage.value =
      err.response?.data?.error || "Failed to send reset link. Try again.";
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
    /* Layout */
    .forgot-page {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 100vh;
        background: #f4f6f8;
    }

    .forgot-card {
        background: white;
        padding: 2rem;
        border-radius: 12px;
        box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
        width: 350px;
        text-align: center;
    }

    h2 {
        color: #2c3e50;
        margin-bottom: 0.5rem;
    }

    .subtitle {
        font-size: 0.9rem;
        color: #555;
        margin-bottom: 1.5rem;
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

    .back-link {
        display: inline-block;
        margin-top: 1.5rem;
        color: #2980b9;
        text-decoration: none;
        font-size: 0.9rem;
    }

        .back-link:hover {
            text-decoration: underline;
        }
</style>
