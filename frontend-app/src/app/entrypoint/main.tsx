import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { RouterProvider, createRouter } from '@tanstack/react-router'

import { routeTree } from '../../routeTree.gen'
import { Provider } from '../providers/Provider'
import { useAuth } from '@/entities/auth/api/providers/AuthProvider'

const router = createRouter({
  routeTree,
  context: {
    auth: null,

  },
})
declare module '@tanstack/react-router' {
  interface Register {
    router: typeof router
  }
}

function InnerApp() {
  const authContextValue = useAuth();
  return <RouterProvider router={router} context={{ auth: authContextValue }} />
}

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider>
      <InnerApp />
    </Provider>
  </StrictMode>
)
