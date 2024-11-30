import { useQuery } from '@tanstack/react-query'
import movieClient from '../../api/movieClient/movieClient'
import { ApiError, ApiResponse, Movie } from '../../api/movieClient/types'

export const useGetMovies = (query: string, page: number = 1) =>
  useQuery<ApiResponse<Movie> | ApiError, ApiError, ApiResponse<Movie>>({
    queryKey: ['getMovies', query, page],
    queryFn: () => movieClient.getMovies(query, page),
    enabled: !!query,
  })
