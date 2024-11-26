<template>
    <v-container>
        <v-row>
            <v-col cols="12">
                <v-card class="mb-6">
                    <v-card-text class="d-flex align-center">
                        <v-avatar size="100" color="grey" class="mr-6">
                            <v-img v-if="user.avatar" :src="user.avatar" alt="Avatar" />
                            <span v-else class="text-h4 white--text">{{ user.initials }}</span>
                        </v-avatar>
                        <div>
                            <h1 class="text-h4 mb-2">{{ user.name }}</h1>
                            <p class="text-subtitle-1 grey--text">{{ user.email }}</p>
                        </div>
                        <v-spacer></v-spacer>
                        <v-btn color="primary" disabled>
                            Editar Perfil
                            <v-icon right>mdi-pencil</v-icon>
                        </v-btn>
                    </v-card-text>
                </v-card>
            </v-col>

            <v-col cols="12" md="3">
                <v-card>
                    <v-list nav>
                        <v-list-item-group v-model="selectedSection" color="primary">
                            <v-list-item v-for="(item, i) in sections" :key="i" :value="item.value"
                                :disabled="['preferences', 'security'].indexOf(item.value) !== -1"
                                @click="item.value === 'logout' ? logout() : selectedSection = item.value">
                                <v-list-item-icon>
                                    <v-icon>{{ item.icon }}</v-icon>
                                </v-list-item-icon>

                                <v-list-item-content>
                                    <v-list-item-title>{{ item.title }}</v-list-item-title>
                                </v-list-item-content>
                            </v-list-item>
                        </v-list-item-group>
                    </v-list>
                </v-card>
            </v-col>

            <v-col cols="12" md="9">
                <v-card v-if="selectedSection === 'personal'" class="mb-6">
                    <v-card-title class="text-h6">
                        Informações Pessoais
                        <v-spacer></v-spacer>
                        <v-btn text color="primary" disabled>
                            <v-icon left>mdi-pencil</v-icon>
                            Editar
                        </v-btn>
                    </v-card-title>
                    <v-card-text>
                        <v-row>
                            <v-col cols="12" sm="6">
                                <v-text-field v-model="user.name" label="Nome Completo" readonly outlined
                                    dense></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6">
                                <v-text-field v-model="user.email" label="Email" readonly outlined dense></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6">
                                <v-text-field v-model="user.phone" label="Telefone" readonly outlined
                                    dense></v-text-field>
                            </v-col>
                            <v-col cols="12" sm="6">
                                <v-text-field v-model="user.location" label="Localização" readonly outlined
                                    dense></v-text-field>
                            </v-col>
                        </v-row>
                    </v-card-text>
                </v-card>

                <v-card v-if="selectedSection === 'preferences'" class="mb-6">
                    
                </v-card>

                <!-- Segurança -->
                <v-card v-if="selectedSection === 'security'" class="mb-6">

                </v-card>


            </v-col>
        </v-row>



    </v-container>
</template>

<script>
import { authService } from '~/services/api.service';

export default {
    name: 'AccountDetails',
    middleware: 'auth',

    data() {
        return {
            selectedSection: 'personal',
            sections: [
                { title: 'Informações Pessoais', icon: 'mdi-account', value: 'personal' },
                { title: 'Preferências', icon: 'mdi-cog', value: 'preferences' },
                { title: 'Segurança', icon: 'mdi-shield-lock', value: 'security' },
                { title: 'Sair', icon: 'mdi-logout', value: 'logout' }
            ],
            user: {
                name: 'ADM',
                email: 'adm@business.com',
                phone: '(01) 92345-6789',
                location: 'Brasil',
                avatar: null,
                initials: 'ADM',
                lastPasswordUpdate: '15/03/2024'
            },
            preferences: {
                emailNotifications: true,
                darkTheme: false
            },


        }
    },

    methods: {
        async logout() {

            try {
                await authService.logout();

                this.$store.dispatch('auth/checkAuthStatus');

                await this.$router.push('/login');
            } catch (error) {
                console.error('Erro ao fazer logout:', error);

            }
        },
        changePassword() {
            console.log('Alterar senha')
        },
        enable2FA() {
            console.log('Ativar 2FA')
        },
        async savePersonalInfo() {
            try {
                console.log('Salvando informações pessoais...')
            } catch (error) {
                console.error('Erro ao salvar:', error)
            }
        }
    }
}
</script>

<style scoped>
.v-card {
    border-radius: 8px;
}

.v-list-item {
    min-height: 64px;
}

.v-list-item--disabled {
    opacity: 0.5;
    cursor: not-allowed;
}

.v-btn.v-btn--disabled {
    opacity: 0.5;
    cursor: not-allowed;
    pointer-events: none;
}

.v-btn--disabled .v-icon {
    opacity: 0.5;
}
</style>