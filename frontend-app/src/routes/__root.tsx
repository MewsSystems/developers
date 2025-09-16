import { RootLayout } from '@/pages/RootLayout'
import type { AuthContext } from '@/entities/auth/api/providers/AuthProvider'
import { createRootRouteWithContext } from '@tanstack/react-router'

interface MyRouterContext {
  auth: AuthContext | null
}

export const Route = createRootRouteWithContext<MyRouterContext>()({ component: RootLayout })