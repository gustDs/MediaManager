<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
import * as authService from '../services/authService'

const router = useRouter()

const email = ref('')
const password = ref('')
const error = ref(null)
const loading = ref(false)

async function submit() {
  error.value = null
  loading.value = true
  try {
    const { token } = await authService.login(email.value, password.value)
    authService.saveToken(token)
    router.push('/')
  } catch (e) {
    error.value = e.message
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <div style="min-height: 100vh; display: flex; align-items: center; justify-content: center; background: var(--surface-0);">
    <div style="width: 360px; background: var(--surface-1); border: 0.5px solid var(--border); border-radius: 16px; padding: 32px;">
      <div style="text-align: center; margin-bottom: 28px;">
        <div style="font-size: 20px; font-weight: 700; color: #e2e8f0;">Media Manager</div>
        <div style="font-size: 13px; color: #64748b; margin-top: 4px;">Entre para continuar</div>
      </div>

      <div style="display: flex; flex-direction: column; gap: 16px;">
        <div style="display: flex; flex-direction: column; gap: 6px;">
          <label style="font-size: 13px; color: #94a3b8;">Email</label>
          <InputText v-model="email" type="email" placeholder="seu@email.com" style="width: 100%;" @keyup.enter="submit" />
        </div>
        <div style="display: flex; flex-direction: column; gap: 6px;">
          <label style="font-size: 13px; color: #94a3b8;">Senha</label>
          <InputText v-model="password" type="password" placeholder="••••••••" style="width: 100%;" @keyup.enter="submit" />
        </div>

        <div v-if="error" style="font-size: 13px; color: #f87171; background: rgba(248,113,113,0.08); border: 0.5px solid rgba(248,113,113,0.2); border-radius: 8px; padding: 10px 12px;">
          {{ error }}
        </div>

        <Button label="Entrar" :loading="loading" style="width: 100%; margin-top: 4px;" @click="submit" />
      </div>
    </div>
  </div>
</template>
