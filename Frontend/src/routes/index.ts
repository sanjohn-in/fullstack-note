import { Session } from '@/utils/storage'
import NProgress from 'nprogress'
import 'nprogress/nprogress.css'
import { createRouter, createWebHashHistory } from 'vue-router'
import { adminRoutes, notFoundAndNoPower, staticRoutes } from './routes'

export const router = createRouter({
  history: createWebHashHistory(),
  routes: [...staticRoutes, ...adminRoutes, ...notFoundAndNoPower],
})

NProgress.configure({ showSpinner: false })

// Public routes that don't need auth
const publicRoutes = ['/login', '/register']

router.beforeEach((to) => {
  NProgress.start()

  const token = Session.get('token')

  if (token) {
    // Authenticated - redirect away from login/register
    if (publicRoutes.includes(to.path)) {
      return { path: '/dashboard' }
    }
    return true
  } else {
    // Not authenticated
    if (publicRoutes.includes(to.path)) {
      return true
    }
    return { path: '/login' }
  }
})

router.afterEach(() => {
  NProgress.done()
})

export default router
