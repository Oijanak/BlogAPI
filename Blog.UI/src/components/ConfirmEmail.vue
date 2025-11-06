<template>
    <div class="container mt-5">
        <div class="card mx-auto text-center p-4" style="max-width: 520px;">
            <h3 class="mb-3">Email Confirmation</h3>

            <div v-if="loading" class="text-muted">
                <div class="spinner-border spinner-border-sm me-2" role="status"></div>
                Confirming your email, please wait...
            </div>

            <div v-else>
                <div v-if="success" class="alert alert-success mt-3">
                    ✅ {{ message }}
                </div>

                <div v-else class="alert alert-danger mt-3">
                    ❌ {{ message }}
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue'
    import { useRoute } from 'vue-router'
    import { useAuthStore } from '../stores/auth'

    const route = useRoute()
    const auth = useAuthStore();
    const loading = ref(true)
    const success = ref(false)
    const message = ref('')
    onMounted(async () => {
        const userId = route.query.userId
        const token = route.query.token

        if (!userId || !token) {
            message.value = 'Invalid confirmation link.'
            loading.value = false
            return
        }

        try {
            const resp = await auth.confirmEmail({
                userId, token
            })
        

    success.value = true
    message.value = 'Your email has been confirmed successfully.'
  } catch (err) {
      if (err.response?.data?.error) {
          message.value = err.response.data.error;
      }
    } finally {
        loading.value = false
    }
})
</script>