import { authService } from "~/services/api.service";

export default async function ({ redirect, route }) {
  const publicRoutes = ['/', '/login'];

  if (route.path === '/login') {
    try {
      const response = await authService.checkAuthStatus();

      if (response.status === 200) {
        return redirect('/account');
      }
    } catch {
      // User is not authenticated, can stay on login page
    }
  }
  
  if (!publicRoutes.includes(route.path)) {
    try {
      const response = await authService.checkAuthStatus();

      if (response.status !== 200) {
        return redirect('/login');
      }
    } catch {
      return redirect('/login');
    }
  }
}
