import { useQuery } from '@tanstack/react-query'

import { api } from '@/api/index'
import { movieKeys } from '@/api/queryKeys'
import { type MovieList, MovieListSchema } from '@/schemas/movie'

const getAllMovies = async () => {
  const response = await api.get('discover/movie')
  const data = await response.json()

  return MovieListSchema.parse(data)
}

export const useMovies = () => {
  return useQuery<MovieList>({
    queryKey: movieKeys.all,
    queryFn: getAllMovies,
  })
}
