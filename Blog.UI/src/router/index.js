import { createRouter, createWebHistory } from 'vue-router'
import Login from "@/components/Login.vue";
import Register from "@/components/Register.vue";
import Blog from "@/components/Blog.vue";
import ConfirmEmail from "@/components/ConfirmEmail.vue";
import ForgetPassword from "@/components/ForgetPassword.vue";
import ResetPassword from "@/components/ResetPassword.vue";
import {useAuthStore} from "@/stores/auth.js";




const routes = [
    { path: '/', component: Register },
    { path: '/register', component: Register },
    { path: '/login', component: Login },
    { path: '/confirm-email', component: ConfirmEmail },
    { path: '/forgot-password', component: ForgetPassword },
    { path: '/reset-password', component: ResetPassword },
    { path: '/blogs', component: Blog,meta:{requiresAuth:true} },
]
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})
router.beforeEach((to, from, next) => {
    const auth = useAuthStore()

    if (to.meta.requiresAuth && !auth.isAuthenticated) {
        next('/login')
    } else {
        next()
    }
})



export default router
