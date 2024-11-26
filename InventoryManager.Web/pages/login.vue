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
                            <login-form 
                                @login-success="handleLoginSuccess"
                                @login-error="handleLoginError"
                            />
                        </v-tab-item>

                        <v-tab-item>
                            <register-form
                                @register-success="handleRegisterSuccess"
                                @register-error="handleRegisterError"
                            />
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
import LoginForm from '~/components/login/LoginForm.vue'
import RegisterForm from '~/components/login/RegisterForm.vue'

export default {
    name: 'AuthPage',
    layout: 'auth',
    components: {
        LoginForm,
        RegisterForm
    },
    data() {
        return {
            activeTab: 0,
            snackbar: false,
            snackbarText: '',
            snackbarColor: 'success'
        }
    },
    methods: {
        handleLoginSuccess() {
            this.snackbarColor = 'success'
            this.snackbarText = 'Bem-vindo!'
            this.snackbar = true
        },
        handleLoginError(errorMessage) {
            this.snackbarColor = 'error'
            this.snackbarText = errorMessage
            this.snackbar = true
        },
        handleRegisterSuccess() {
            this.snackbarColor = 'success'
            this.snackbarText = 'Cadastro realizado com sucesso!'
            this.snackbar = true
            this.activeTab = 0
        },
        handleRegisterError(errorMessage) {
            this.snackbarColor = 'error'
            this.snackbarText = errorMessage
            this.snackbar = true
        }
    }
}
</script>

<style scoped>
.auth-card {
    margin: auto 0;
    border-radius: 8px;
}
</style>