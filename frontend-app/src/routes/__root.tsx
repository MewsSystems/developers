import { RootLayout } from '@/pages/RootLayout'
import type { AuthContextType } from '@/entities/auth/api/providers/AuthProvider'
import { createRootRouteWithContext } from '@tanstack/react-router'

interface MyRouterContext {
  auth: AuthContextType | null
}

export const Route = createRootRouteWithContext<MyRouterContext>()({ component: RootLayout })