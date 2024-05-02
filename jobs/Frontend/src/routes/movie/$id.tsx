import { createFileRoute } from '@tanstack/react-router'

import { routeNames } from '@/constants/routeNames'
import Movie from '@/components/Movie'

export const Route = createFileRoute(routeNames.movie)({
	component: Movie
})