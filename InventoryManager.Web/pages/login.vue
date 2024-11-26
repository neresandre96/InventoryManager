<template>
    <v-container fluid fill-height>
        <v-row align="center" justify="center">
            <v-col cols="12" sm="8" md="6" lg="4">
                <v-card class="elevation-12 auth-card">
                    <v-tabs v-model="activeTab" background-color="primary" dark grow>
                        <v-tab>Login</v-tab>
                        <v-tab>Cadastro</v-tab>
                    </v-tabs>

                    <v-tabs-items v-model="activeTab">

                        <v-tab-item>
                            <v-card-text class="pt-6">
                                <v-form ref="loginForm" v-model="loginValid" @submit.prevent="handleLogin">
                                    <v-text-field v-model="loginData.email" :rules="emailRules" label="E-mail"
                                        prepend-icon="mdi-email" type="email" required autocomplete="email" dense />

                                    <v-text-field v-model="loginData.password" :rules="passwordRules" label="Senha"
                                        prepend-icon="mdi-lock"
                                        :append-icon="showLoginPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                        :type="showLoginPassword ? 'text' : 'password'"
                                        @click:append="showLoginPassword = !showLoginPassword" required
                                        autocomplete="current-password" dense />

                                    <div class="text-right">
                                        <v-btn text small color="primary" class="mb-4">
                                            Esqueceu a senha?
                                        </v-btn>
                                    </div>

                                    <v-btn block color="primary" :loading="loading" :disabled="!loginValid || loading"
                                        height="44" class="btn-submit" @click="handleLogin">
                                        Entrar
                                    </v-btn>
                                </v-form>
                            </v-card-text>
                        </v-tab-item>


                        <v-tab-item>
                            <v-card-text class="pt-6">
                                <v-form ref="registerForm" v-model="registerValid" @submit.prevent="handleRegister">

                                    <v-text-field v-model="registerData.email" :rules="emailRules" label="E-mail"
                                        prepend-icon="mdi-email" type="email" required dense />

                                    <v-text-field v-model="registerData.password" :rules="passwordRules" label="Senha"
                                        prepend-icon="mdi-lock"
                                        :append-icon="showRegisterPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                        :type="showRegisterPassword ? 'text' : 'password'"
                                        @click:append="showRegisterPassword = !showRegisterPassword" required dense />

                                    <v-text-field v-model="registerData.confirmPassword" :rules="confirmPasswordRules"
                                        label="Confirmar senha" prepend-icon="mdi-lock-check"
                                        :append-icon="showConfirmPassword ? 'mdi-eye' : 'mdi-eye-off'"
                                        :type="showConfirmPassword ? 'text' : 'password'"
                                        @click:append="showConfirmPassword = !showConfirmPassword" required dense />

                                    <v-btn block color="primary" :loading="loading"
                                        :disabled="!registerValid || loading" height="44" class="btn-submit"
                                        @click="handleRegister">
                                        Cadastrar
                                    </v-btn>
                                </v-form>
                            </v-card-text>
                        </v-tab-item>
                    </v-tabs-items>
                </v-card>
            </v-col>
        </v-row>

        <v-snackbar v-model="snackbar" :color="snackbarColor" timeout="3000">
            {{ snackbarText }}
        </v-snackbar>
    </v-container>
</template>

<script>
import axios from 'axios';

export default {
    name: 'AuthPage',
    layout: 'auth',
    data() {
        return {
            activeTab: 0,
            loading: false,
            loginValid: false,
            registerValid: false,
            showLoginPassword: false,
            showRegisterPassword: false,
            showConfirmPassword: false,
            snackbar: false,
            snackbarText: '',
            snackbarColor: 'success',

            loginData: {
                email: '',
                password: ''
            },

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
        async handleLogin() {
            if (!this.$refs.loginForm.validate()) return;

            this.loading = true;
            try {
                const response = await axios.post('http://localhost:5165/auth/login?useCookies=true', {
                    email: this.loginData.email,
                    password: this.loginData.password
                }, {
                    withCredentials: true
                });

                this.snackbarColor = 'success';
                this.snackbarText = `Bem-vindo!`;
                this.snackbar = true;

                this.$store.dispatch('auth/checkAuthStatus');

                this.$router.push('/');
            } catch (error) {

                console.error('Erro ao realizar login:', error);
                this.snackbarColor = 'error';
                this.snackbarText = error.response?.data?.message || 'Erro ao realizar login. Verifique suas credenciais.';
                this.snackbar = true;
            } finally {
                this.loading = false;
            }
        },

        async handleRegister() {
            if (!this.$refs.registerForm.validate()) return

            this.loading = true
            try {
                const response = await axios.post('http://localhost:5165/auth/register', {
                    email: this.registerData.email,
                    password: this.registerData.password
                });

                this.snackbarColor = 'success';
                this.snackbarText = `Cadastro realizado com sucesso!`;
                this.snackbar = true;

                this.$store.dispatch('auth/checkAuthStatus');

                this.activeTab = 0
            } catch (error) {
                this.snackbarColor = 'error'
                this.snackbarText = 'Erro ao realizar cadastro. Tente novamente.'
                this.snackbar = true
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

.auth-card {
    margin: auto 0;
    border-radius: 8px;
}

.v-text-field {
    margin-top: 35px;
    margin-bottom: 8px;
}
</style>