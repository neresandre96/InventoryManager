<template>
  <v-card class="py-8 mx-auto" width="60%">
    <p>Autenticação fictícia com email e senha:</p>
    <p class="chips">
      <b>demo@teste.com</b><br />
    </p>
    <p class="chips">
      <b>Demo@123</b>
    </p>
    <v-btn :disabled="isAuthenticated" color="primary" @click="handleAutoLogin">
      {{ isAuthenticated ? 'Sessão já ativa' : 'Entrar com esta conta' }}
    </v-btn>
  </v-card>
</template>

<script>
import { authService } from '@/services/api.service'

export default {
  name: 'DemoLogin',
  props: {
    isAuthenticated: {
      type: Boolean,
      default: false
    }
  },
  methods: {
    async handleAutoLogin() {
      try {
        const response = await authService.login({
          email: 'demo@teste.com',
          password: 'Demo@123'
        })

        if (response.status === 200) {
          this.$store.dispatch('auth/checkAuthStatus')
          this.$router.push('/inventory')
        }
      } catch (error) {
        console.error('Erro ao tentar fazer login automático:', error)
      }
    }
  }
}
</script>