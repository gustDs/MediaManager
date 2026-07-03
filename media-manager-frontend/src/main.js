import { createApp } from 'vue'
import PrimeVue from 'primevue/config'
import ConfirmationService from 'primevue/confirmationservice'
import Aura from '@primevue/themes/aura'
import 'primeicons/primeicons.css'
import './style.css'
import router from './router'
import App from './App.vue'

document.documentElement.classList.add('dark')

createApp(App)
  .use(PrimeVue, {
    theme: {
      preset: Aura,
      options: {
        darkModeSelector: 'html.dark',
      },
    },
  })
  .use(ConfirmationService)
  .use(router)
  .mount('#app')
