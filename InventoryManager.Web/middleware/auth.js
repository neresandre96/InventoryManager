export default async function ({ redirect, route, $axios }) {
  const publicRoutes = ['/', '/login'];

  if (route.path === '/login') {
    try {
      const response = await $axios.get('http://localhost:5165/auth/manage/info', {
        withCredentials: true,
      });

      if (response.status === 200) {
        return redirect('/account');
      }
    } catch {
      // User is not authenticated, can stay on login page
    }
  }
  
  if (!publicRoutes.includes(route.path)) {
    try {
      const response = await $axios.get('http://localhost:5165/auth/manage/info', {
        withCredentials: true,
      });

      if (response.status !== 200) {
        return redirect('/login');
      }
    } catch {
      return redirect('/login');
    }
  }
}
