<template>
  <v-container class="py-8">

    <v-row justify="center" align="center" class="mb-8">
      <v-col cols="12" md="8" class="text-center">
        <h1 class="font-weight-bold">Projeto de Gerenciamento de Estoque</h1>
        <p>
          Este é um protótipo de um sistema de gerenciamento de estoque idealizado para o teste de desenvolvedor de
          software da ZDZCode.
        </p>

        <demo-login :is-authenticated="isAuthenticated" />

      </v-col>
    </v-row>
    
    <v-row class="mb-8">
      <v-col cols="12" class="text-center">
        <h2 class="font-weight-bold">Funcionalidades</h2>
      </v-col>

      <features-card title="🪪 Autenticação" message="A solução é protegida por autenticação e autorização, tanto a nível de API quanto de Cliente." />

      <features-card  title="🔑 Multi-tenant" message="Cada usuário tem seu próprio ambiente de gerenciamento e dados de estoque gerenciado por tokens.
            Você pode criar outros usuários com email fictícios." />

      <features-card title="📦 Controle de Estoque" message="Gerencie a entrada e saída de ingredientes com um sistema simples e eficiente.
            Controle os pesos e valores associados a cada item." />

      <features-card title="🔍 Busca e Filtros" message="Utilize a funcionalidade de busca para localizar ingredientes rapidamente. Os dados são exibidos em uma tabela dinâmica e intuitiva." />

      <features-card title="📊 Relatórios" message="Visualize o histórico de movimentações de estoque." />

      <features-card title="🛒 Lista de compras" message="Gere rapidamente uma lista de compras baseada nos ingredientes em falta." />

    </v-row>

  </v-container>
</template>

<script>
import DemoLogin from '~/components/home/DemoLogin.vue'
import FeaturesCard from '~/components/home/FeaturesCard.vue';

export default {
  name: "IndexPage",
  components: {
    DemoLogin,
    FeaturesCard,
  },
  data() {
    return {
      isAuthenticated: false,
    }
  },
  created() {
    this.checkAuthStatus()
  },
  methods: {
    checkAuthStatus() {
      this.isAuthenticated = this.$store.state.auth.isLoggedIn
    }
  },
  watch: {
    '$store.state.auth.isLoggedIn'(newValue) {
      this.isAuthenticated = newValue
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
