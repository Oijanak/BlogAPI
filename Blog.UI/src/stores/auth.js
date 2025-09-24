import { defineStore } from 'pinia'
import axios from 'axios'

const API_URL = "http://localhost:5058/api/Auth"

export const useAuthStore = defineStore('auth', {
    state: () => ({
        accessToken: localStorage.getItem('accessToken') || null,
        refreshToken: localStorage.getItem('refreshToken') || null,
    }),
    getters: {
        isAuthenticated: state => !!state.accessToken,
    },

    actions: {
        async register(payload) {
            await axios.post(`${API_URL}/register`, payload)
        },

        async login(payload) {
            const res = await axios.post(`${API_URL}/login`, payload)
            this.accessToken = res.data.accessToken
            this.refreshToken = res.data.refreshToken
            localStorage.setItem('accessToken', res.data.accessToken)
            localStorage.setItem('refreshToken', res.data.refreshToken)
        },

        async refresh() {
            const res = await axios.post(`${API_URL}/refresh`, { accessToken:this.accessToken,refreshToken: this.refreshToken })
            this.accessToken = res.data.accessToken
            this.refreshToken = res.data.refreshToken
            localStorage.setItem('accessToken', this.data.accessToken)
            localStorage.setItem("refreshToken", res.data.refreshToken)
        },

        logout() {
            this.accessToken = null
            this.refreshToken = null
            localStorage.clear()
        }
    }
})

