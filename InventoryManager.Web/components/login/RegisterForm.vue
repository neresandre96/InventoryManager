<template>
    <v-card-text class="pt-6">
      <v-form ref="registerForm" v-model="registerValid" @submit.prevent="handleRegister">
        <v-text-field 
          v-model="registerData.email" 
          :rules="emailRules" 
          label="E-mail"
          prepend-icon="mdi-email" 
          type="email" 
          required 
          dense 
        />
  
        <v-text-field 
          v-model="registerData.password" 
          :rules="passwordRules" 
          label="Senha"
          prepend-icon="mdi-lock"
          :append-icon="showRegisterPassword ? 'mdi-eye' : 'mdi-eye-off'"
          :type="showRegisterPassword ? 'text' : 'password'"
          @click:append="showRegisterPassword = !showRegisterPassword" 
          required 
          dense 
        />
  
        <v-text-field 
          v-model="registerData.confirmPassword" 
          :rules="confirmPasswordRules"
          label="Confirmar senha" 
          prepend-icon="mdi-lock-check"
          :append-icon="showConfirmPassword ? 'mdi-eye' : 'mdi-eye-off'"
          :type="showConfirmPassword ? 'text' : 'password'"
          @click:append="showConfirmPassword = !showConfirmPassword" 
          required 
          dense 
        />
  
        <v-btn 
          block 
          color="primary" 
          :loading="loading"
          :disabled="!registerValid || loading" 
          height="44" 
          class="btn-submit"
          @click="handleRegister"
        >
          Cadastrar
        </v-btn>
      </v-form>
    </v-card-text>
  </template>
  
  <script>
  import { authService } from '@/services/api.service'
  
  export default {
    name: 'RegisterForm',
    data() {
      return {
        loading: false,
        registerValid: false,
        showRegisterPassword: false,
        showConfirmPassword: false,
        registerData: {
          email: '',
          password: '',
          confirmPassword: ''
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
        ],
        confirmPasswordRules: [
          v => !!v || 'Confirmação de senha é obrigatória',
          v => v === this.registerData.password || 'As senhas não coincidem'
        ]
      }
    },
    methods: {
      async handleRegister() {
        if (!this.$refs.registerForm.validate()) return
  
        this.loading = true
        try {
          await authService.register({
            email: this.registerData.email,
            password: this.registerData.password
          })
          
          this.$emit('register-success')
        } catch (error) {
          this.$emit('register-error', 'Erro ao realizar cadastro. Tente novamente.')
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
  