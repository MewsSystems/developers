import { createFileRoute, redirect } from '@tanstack/react-router'
import { AuthLayout } from '@/pages/auth/AuthLayout'

export const Route = createFileRoute('/_auth')({
  beforeLoad: ({ context, location }) => {
    if (!context.auth?.isAuthenticated) {
      throw redirect({
        to: '/login',
        search: {
          redirect: location.href,
        },
      })
    }
  },
  component: AuthLayout,
})