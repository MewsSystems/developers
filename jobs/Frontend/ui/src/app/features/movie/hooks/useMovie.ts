import { useQuery } from "@tanstack/react-query"

import NextApiService from "@/app/services/NextApi.service"

export const useMovie = (movieId: string) => {
  return useQuery({
    queryKey: ["movie", movieId],
    queryFn: () => NextApiService.getMovie(movieId),
    staleTime: 2 * 60 * 1000,
  })
}
