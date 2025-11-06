import { defineStore } from 'pinia'
import axios from 'axios'
import { jwtDecode } from 'jwt-decode';

const API_URL = "http://localhost:5058/api/Auth"

export const useAuthStore = defineStore('auth', {
    state: () => ({
        accessToken: localStorage.getItem('accessToken') || null,
        refreshToken: localStorage.getItem('refreshToken') || null,
    }),
    getters: {
        isAuthenticated: state => !!state.accessToken,
         currentUser: (state) => {
    if (!state.accessToken) return null;
    try {
        
      const decoded = jwtDecode(state.accessToken);
      const role=decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
        const email = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]
      return {role,email}; 
    } catch (err) {
      return null;
    }
  }
    },

    actions: {
        async register(payload) {
            await axios.post(`${API_URL}/register`, payload)
        },

        async confirmEmail(payload) {
            await axios.post(`${API_URL}/confirm-email`,payload)
        },

        async login(payload) {
            const res = await axios.post(`${API_URL}/login`, payload)
            this.accessToken = res.data.data.accessToken
            this.refreshToken = res.data.data.refreshToken
            localStorage.setItem('accessToken', res.data.accessToken)
            localStorage.setItem('refreshToken', res.data.refreshToken)
        },

        async refresh() {
            const res = await axios.post(`${API_URL}/refresh`, { accessToken:this.accessToken,refreshToken: this.refreshToken })
            this.accessToken = res.data.data.accessToken
            this.refreshToken = res.data.data.refreshToken
            localStorage.setItem('accessToken', res.data.accessToken)
            localStorage.setItem("refreshToken", res.data.refreshToken)
        },

        logout() {
            this.accessToken = null
            this.refreshToken = null
            localStorage.clear()
        }
    }
})

