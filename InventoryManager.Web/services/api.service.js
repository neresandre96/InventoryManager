import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5165/api",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

const authClient = axios.create({
  baseURL: "http://localhost:5165/auth",
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

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
    return apiClient.get("/Ingredient");
  },

  getMovements() {
    return apiClient.get("/Movement");
  },

  createIngredient(data) {
    return apiClient.post("/Ingredient", data);
  },

  updateIngredient(id, data) {
    return apiClient.put(`/Ingredient/${id}`, data);
  },

  deleteIngredient(id) {
    return apiClient.delete(`/Ingredient/${id}`);
  },

  createMovement(data) {
    return apiClient.post("/Movement", data);
  },
};
