import { authService } from "~/services/api.service";

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
      const response = await authService.checkAuthStatus();
      commit('setAuthState', response.status === 200);
    } catch {
      commit('setAuthState', false);
    }
  },
};
