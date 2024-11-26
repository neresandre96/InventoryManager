<template>
  <v-app dark>
    <v-navigation-drawer v-model="drawer" :mini-variant="miniVariant" :clipped="clipped" fixed app>
      <v-list>
        <v-list-item v-for="(item, i) in items" :key="i" :to="item.to" router exact>
          <v-list-item-action>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar :clipped-left="clipped" fixed app>
      <v-btn icon @click.stop="miniVariant = !miniVariant">
        <v-icon>mdi-{{ `chevron-${miniVariant ? 'right' : 'left'}` }}</v-icon>
      </v-btn>

      <v-toolbar-title>{{ title }}</v-toolbar-title>

      <v-spacer />

      <v-btn v-if="isLoggedIn" icon to="/account">
        <v-icon>mdi-account</v-icon>
      </v-btn>
      <v-btn v-else color="primary" @click="goToLogin">
        Acessar
      </v-btn>

    </v-app-bar>

    <v-main class="main-content">
      <v-container>
        <Nuxt />
      </v-container>
    </v-main>

    <v-footer :absolute="!fixed" app class="footer">
      <span>&copy; {{ new Date().getFullYear() }} Andre Mariano Neres</span>
    </v-footer>
  </v-app>
</template>

<script>
import axios from 'axios';

export default {
  name: 'DefaultLayout',
  data() {
    return {
      clipped: true,
      drawer: false,
      fixed: true,
      items: [
        {
          icon: 'mdi-home',
          title: 'Inicio',
          to: '/'
        },
        {
          icon: 'mdi-package',
          title: 'Estoque',
          to: '/inventory'
        }
      ],
      miniVariant: true,
      title: 'ZDZCode - Inventory Manager'
    };
  },
  computed: {
    isLoggedIn() {
      return this.$store.state.auth.isLoggedIn;
    },
  },
  methods: {
    goToLogin() {
      this.$router.push('/login');
    },
  },
  created() {
    this.$store.dispatch('auth/checkAuthStatus');
  },
};
</script>

<style scoped>
.v-application {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  background: rgb(113, 113, 113);
}

.main-content {
  height: 100%;
}

.footer {
  flex-shrink: 0;
  align-items: center;
  justify-content: center;
  font-size: 0.8rem;
}
</style>
