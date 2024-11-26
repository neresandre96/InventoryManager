import axios from "axios";
require('dotenv').config()

const apiClient = axios.create({
  
  baseURL: process.env.VUE_APP_API_URL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

const authClient = axios.create({
  baseURL: process.env.VUE_APP_AUTH_URL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

async function makeRequest(method, url, data = null) {
  try {
    const config = data ? { method, url, data } : { method, url };
    const response = await apiClient(config);
    
    if (response.status >= 200 && response.status < 300) {
      return response.data;
    }
    throw new Error(`Erro inesperado: ${response.status}`);
  } catch (error) {
    throw new Error(error.response?.data?.message || error.message || 'Erro desconhecido');
  }
}

export const authService = {
  login(data) {
    return authClient.post("/login?useCookies=true", data);
  },

  register(data) {
    return authClient.post("/register", data);
  },

  checkAuthStatus() {
    return authClient.get("/manage/info");
  },

  logout() {
    return authClient.post("/logout", {}, { withCredentials: true });
  },
};

export const inventoryService = {
  getIngredients() {
    return makeRequest('get', '/Ingredient');
  },

  getMovements() {
    return makeRequest('get', '/Movement');
  },

  createIngredient(ingredientData) {
    return makeRequest('post', '/Ingredient', ingredientData);
  },

  updateIngredient(id, data) {
    return makeRequest('put', `/Ingredient/${id}`, data);
  },

  deleteIngredient(id) {
    return makeRequest('delete', `/Ingredient/${id}`);
  },

  createMovement(data) {
    return makeRequest('post', '/Movement', data);
  },
};
