<template>
  <v-container class="py-8">

    <v-row justify="center" align="center" class="mb-8">
      <v-col cols="12" md="8" class="text-center">
        <h1 class="font-weight-bold">Projeto de Gerenciamento de Estoque</h1>
        <p>
          Este é um protótipo de um sistema de gerenciamento de estoque idealizado para o teste de desenvolvedor de
          software da ZDZCode.
        </p>

        <v-card class="py-8 mx-auto" width="60%">
          <p>Autenticação fictícia com email e senha:</p>
          <p class="chips">
            <b>demo@teste.com</b><br />
          </p>
          <p class="chips">
            <b>123456</b>
          </p>
          <v-btn :disabled="isAuthenticated" color="primary" @click="autoLogin">
            {{ isAuthenticated ? 'Sessão já ativa' : 'Entrar com esta conta' }}
          </v-btn>

        </v-card>

      </v-col>
    </v-row>


    <v-row class="mb-8">
      <v-col cols="12" class="text-center">
        <h2 class="font-weight-bold">Funcionalidades</h2>
      </v-col>

      <v-col cols="12" md="6">
        <v-card outlined height="100%">
          <v-card-title>🪪 Autenticação</v-card-title>
          <v-card-text>
            A solução é protegida por autenticação e autorização, tanto a nível de API quanto de Cliente.
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card outlined height="100%">
          <v-card-title>🔑 Multi-tenant</v-card-title>
          <v-card-text>
            Cada usuário tem seu próprio ambiente de gerenciamento e dados de estoque gerenciado por tokens.
            Você pode criar outros usuários com email fictícios.
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card outlined height="100%">
          <v-card-title>📦 Controle de Estoque</v-card-title>
          <v-card-text>
            Gerencie a entrada e saída de ingredientes com um sistema simples e eficiente.
            Controle os pesos e valores associados a cada item.
          </v-card-text>
        </v-card>
      </v-col>
      <v-col cols="12" md="6">
        <v-card outlined height="100%">
          <v-card-title>🔍 Busca e Filtros</v-card-title>
          <v-card-text>
            Utilize a funcionalidade de busca para localizar ingredientes rapidamente.
            Os dados são exibidos em uma tabela dinâmica e intuitiva.
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card outlined height="100%">
          <v-card-title>📊 Relatórios</v-card-title>
          <v-card-text>
            Visualize o histórico de movimentações de estoque.
          </v-card-text>
        </v-card>
      </v-col>

      <v-col cols="12" md="6">
        <v-card outlined height="100%">
          <v-card-title>🛒 Lista de compras</v-card-title>
          <v-card-text>
            Gere rapidamente uma lista de compras baseada nos ingredientes em falta.
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

  </v-container>
</template>

<script>
import axios from 'axios';

export default {
  name: "IndexPage",

  data() {
    return {
      isAuthenticated: false,
    };
  },
  created() {
    this.checkAuthStatus();
  },

  methods: {
    checkAuthStatus() {
      this.isAuthenticated = this.$store.state.auth.isLoggedIn;
    },
    async autoLogin() {
      const email = 'demo@teste.com';
      const password = 'Demo@123';

      try {
        const response = await axios.post('http://localhost:5165/auth/login?useCookies=true', {
          email: email,
          password: password,
        }, {
          withCredentials: true
        });

        if (response.status === 200) {
          this.snackbarColor = 'success';
          this.snackbarText = `Bem-vindo!`;
          this.snackbar = true;

          this.$store.dispatch('auth/checkAuthStatus');

          this.$router.push('/inventory');
        }
      } catch (error) {
        console.error('Erro ao tentar fazer login automático:', error);
        alert('Falha ao tentar logar');
      }
    }
  },
  watch: {
    '$store.state.auth.isLoggedIn'(newValue) {
      this.isAuthenticated = newValue;
    }
  }
};
</script>

<style scoped>
h1 {
  color: #ffffff;
  margin-bottom: 30px;
}

h2 {
  color: #ffffff;
}

.blockquote {
  font-style: italic;
  font-size: 1.2rem;
  margin: 20px 0;
}
</style>
