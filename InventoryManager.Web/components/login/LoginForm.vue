<template>
    <v-card-text class="pt-6">
      <v-form ref="loginForm" v-model="loginValid" @submit.prevent="handleLogin">
        <v-text-field 
          v-model="loginData.email" 
          :rules="emailRules" 
          label="E-mail"
          prepend-icon="mdi-email" 
          type="email" 
          required 
          autocomplete="email" 
          dense 
        />
  
        <v-text-field 
          v-model="loginData.password" 
          :rules="passwordRules" 
          label="Senha"
          prepend-icon="mdi-lock"
          :append-icon="showLoginPassword ? 'mdi-eye' : 'mdi-eye-off'"
          :type="showLoginPassword ? 'text' : 'password'"
          @click:append="showLoginPassword = !showLoginPassword" 
          required
          autocomplete="current-password" 
          dense 
        />
  
        <div class="text-right">
          <v-btn text small color="primary" class="mb-4">
            Esqueceu a senha?
          </v-btn>
        </div>
  
        <v-btn 
          block 
          color="primary" 
          :loading="loading" 
          :disabled="!loginValid || loading"
          height="44" 
          class="btn-submit" 
          @click="handleLogin"
        >
          Entrar
        </v-btn>
      </v-form>
    </v-card-text>
  </template>
  
  <script>
  import { authService } from '@/services/api.service'
  
  export default {
    name: 'LoginForm',
    data() {
      return {
        loading: false,
        loginValid: false,
        showLoginPassword: false,
        loginData: {
          email: '',
          password: ''
        },
        emailRules: [
          v => !!v || 'E-mail é obrigatório',
          v => /.+@.+\..+/.test(v) || 'E-mail deve ser válido'
        ],
        passwordRules: [
          v => !!v || 'Senha é obrigatória',
          v => v.length >= 6 || 'Senha deve ter no mínimo 6 caracteres',
          v => /[A-Z]/.test(v) || 'A senha deve conter pelo menos uma letra maiúscula',
          v => /[a-z]/.test(v) || 'A senha deve conter pelo menos uma letra minúscula',
          v => /\d/.test(v) || 'A senha deve conter pelo menos um número',
          v => /[!@#$%^&*(),.?":{}|<>]/.test(v) || 'A senha deve conter pelo menos um caractere especial'
        ]
      }
    },
    methods: {
      async handleLogin() {
        if (!this.$refs.loginForm.validate()) return
  
        this.loading = true
        try {
          await authService.login({
            email: this.loginData.email,
            password: this.loginData.password
          })
  
          this.$emit('login-success')
          this.$store.dispatch('auth/checkAuthStatus')
          this.$router.push('/')
        } catch (error) {
          this.$emit('login-error', error.response?.data?.message || 'Erro ao realizar login. Verifique suas credenciais.')
        } finally {
          this.loading = false
        }
      }
    }
  }
  </script>
  
  <style scoped>
  .btn-submit {
    margin-top: 25px;
  }
  
  .v-text-field {
    margin-top: 35px;
    margin-bottom: 8px;
  }
  </style>
  