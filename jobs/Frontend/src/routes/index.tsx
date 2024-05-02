import { createFileRoute, Navigate } from '@tanstack/react-router'

import { routeNames } from '@/constants/routeNames'

export const Route = createFileRoute(routeNames.index)({
	component: () => <Navigate search={ { query: '' } } to={routeNames.search} />
})