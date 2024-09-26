import {
  createRouter,
  createRoute,
  createRootRoute,
} from '@tanstack/react-router'
import Detail from 'routes/detail/detail'
import Main from 'routes/main/main'
import Root from 'routes/root/root'

const rootRoute = createRootRoute({
  component: Root,
})

const mainRoute = createRoute({
  getParentRoute: () => rootRoute,
  path: '/',
  component: Main,
})

const detailRoute = createRoute({
  getParentRoute: () => mainRoute,
  path: '/detail/$movieId',
  component: Detail,
})

const routeTree = rootRoute.addChildren([mainRoute.addChildren([detailRoute])])

const router = createRouter({ routeTree })

declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

export default router
