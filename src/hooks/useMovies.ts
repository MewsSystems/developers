import { useQuery } from '@tanstack/react-query'

import { api } from '@/api/index'
import { movieKeys } from '@/api/queryKeys'
import { type MovieDiscovery, MovieDiscoverySchema } from '@/schemas/movie'

const getAllMovies = async (search: string, page: number) => {
  const response = await api.get(`search/movie?query=${search}&page=${page}`)
  const data = await response.json()

  return MovieDiscoverySchema.parse(data)
}

export const useMovies = (search: string, page: number) => {
  return useQuery<MovieDiscovery>({
    queryKey: movieKeys.all,
    queryFn: async () => getAllMovies(search, page),
  })
}
