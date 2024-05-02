import { createFileRoute } from '@tanstack/react-router'

import { routeNames } from '@/constants/routeNames'
import MoviesSearch from '@/components/MoviesSearch'

export const Route = createFileRoute(routeNames.search)({
	validateSearch: (search: Record<string, unknown>): {
	query?: string
	page?: number
} => {
		return {
			query: (search.query as string) || '',
		}
	},
	component: () => <MoviesSearch />
})