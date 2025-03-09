import { useQuery } from '@tanstack/react-query'

import { api } from '@/api/index'
import { movieKeys } from '@/api/queryKeys'
import { MovieDetail, MovieDetailSchema } from '@/schemas/movieDetail'

const getMovieDetail = async (movieId: string) => {
  const response = await api.get(`movie/${movieId}`)
  const data = await response.json()
  console.log(data)
  return MovieDetailSchema.parse(data)
}

export const useMovieDetail = (movieId: string) => {
  return useQuery<MovieDetail>({
    queryKey: [movieKeys.movieDetail, movieId],
    queryFn: () => getMovieDetail(movieId),
  })
}
