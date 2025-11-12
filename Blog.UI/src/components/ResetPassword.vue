<template>
    <div class="reset-page">
        <div class="reset-card">
            <h2>Reset Password</h2>

            <p class="subtitle">
                Please enter your new password below.
            </p>

            <form @submit.prevent="handleResetPassword">
                <input v-model.trim="password"
                       type="password"
                       placeholder="New Password"
                       required />
                <input v-model.trim="confirmPassword"
                       type="password"
                       placeholder="Confirm Password"
                       required />

                <button type="submit" :disabled="loading">
                    {{ loading ? "Resetting..." : "Reset Password" }}
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
import { ref, onMounted } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth";

const route = useRoute();
const router = useRouter();
const auth = useAuthStore();

const userId = ref("");
const token = ref("");
const password = ref("");
const confirmPassword = ref("");
const loading = ref(false);
const successMessage = ref("");
const errorMessage = ref("");

// Get userId and token from query params
onMounted(() => {
  userId.value = route.query.userId || "";
  token.value = route.query.token || "";

  if (!userId.value || !token.value) {
    errorMessage.value = "Invalid or missing reset token.";
  }
});


const handleResetPassword = async () => {
  errorMessage.value = "";
  successMessage.value = "";

  if (password.value !== confirmPassword.value) {
    errorMessage.value = "Passwords do not match.";
    return;
  }

  loading.value = true;
  try {
    await auth.resetPassword({
      userId: userId.value,
      token: token.value,
      newPassword: password.value,
    });

    successMessage.value = "Password reset successful!";
    setTimeout(() => router.push("/login"), 1500);
  } catch (err) {
    console.error("Reset password error:", err);
    errorMessage.value =
      err.response?.data?.error || "Failed to reset password. Try again.";
  } finally {
    loading.value = false;
  }
};
</script>

<style scoped>
    .reset-page {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 100vh;
        background: #f4f6f8;
    }

    .reset-card {
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
