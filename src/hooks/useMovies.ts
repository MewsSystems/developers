import { useQuery } from '@tanstack/react-query'

import { api } from '@/api/index'
import { movieKeys } from '@/api/queryKeys'
import { type MovieDiscovery, MovieDiscoverySchema } from '@/schemas/movie'

const getAllMovies = async () => {
  const response = await api.get('discover/movie')
  const data = await response.json()

  return MovieDiscoverySchema.parse(data)
}

export const useMovies = () => {
  return useQuery<MovieDiscovery>({
    queryKey: movieKeys.all,
    queryFn: getAllMovies,
  })
}
