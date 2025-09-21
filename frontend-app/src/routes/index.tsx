import { MainPageRouteComponent } from '@/pages/main'
import { createFileRoute } from '@tanstack/react-router'

export const Route = createFileRoute("/" as any)({
    component: MainPageRouteComponent,
})
