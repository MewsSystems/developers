import { useInfiniteQuery } from '@tanstack/react-query'
import { useRouterState } from '@tanstack/react-router'

import { trending, search } from '@/api/index'

export const useMovies = () => {
	const router = useRouterState()
	return useInfiniteQuery({
		queryKey: ['movies', { query: router.location.search.query?.trim() }],
		staleTime: Infinity,
		queryFn: ({ pageParam: pageFromInfiniteScroll }) => {
			const page = Number(pageFromInfiniteScroll)
			if (router.location.search.query) return search({
				query: router.location.search.query, 
				page
			})

			/**
			 * When query is missing or empty we search trending movies
			 */
			return trending({ page })
		},
		initialPageParam: 1,
		getNextPageParam: lastPage => {
			if (lastPage.page >= lastPage.total_pages) return null
			return lastPage.page + 1
		}
	})
}