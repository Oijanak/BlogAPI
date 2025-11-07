import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import '@fortawesome/fontawesome-free/css/all.css';
import '@fortawesome/fontawesome-free/js/all.js';
import vue3GoogleLogin from "vue3-google-login";
const app = createApp(App)

app.use(createPinia())

app.use(vue3GoogleLogin, {
    clientId: "855256408648-rnc3jaa37m47inn9scmdfaa7e18pjb8u.apps.googleusercontent.com"
});
app.use(router)

app.mount('#app')

