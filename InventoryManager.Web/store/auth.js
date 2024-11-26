export const state = () => ({
    isLoggedIn: false,
  });
  
  export const mutations = {
    setAuthState(state, status) {
      state.isLoggedIn = status;
    },
  };
  
  export const actions = {
    async checkAuthStatus({ commit }) {
      try {
        const response = await this.$axios.get('http://localhost:5165/auth/manage/info', {
          withCredentials: true,
        });
        commit('setAuthState', response.status === 200);
      } catch {
        commit('setAuthState', false);
      }
    },
  };
  