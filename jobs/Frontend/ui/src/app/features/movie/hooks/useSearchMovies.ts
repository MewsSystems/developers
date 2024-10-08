import { useInfiniteQuery } from "@tanstack/react-query"

import NextApiService from "@/app/services/NextApi.service"

export const useSearchMovies = (query: string) => {
  return useInfiniteQuery({
    queryKey: ["movies", query],
    queryFn: ({ pageParam = 1 }) =>
      NextApiService.searchMovies(query, pageParam),
    initialPageParam: 1,
    getNextPageParam: (lastPage) => {
      if (lastPage.page < lastPage.total_pages) {
        return lastPage.page + 1
      }

      return undefined
    },
    staleTime: 2 * 60 * 1000,
  })
}
