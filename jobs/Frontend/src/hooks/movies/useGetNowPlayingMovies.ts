import { useQuery } from '@tanstack/react-query'
import movieClient from '../../api/movieClient/movieClient'
import {
  ApiError,
  ApiResponse,
  Movie,
} from '../../api/movieClient/movieClientTypes'

export const useGetNowPlayingMovies = () =>
  useQuery<ApiResponse<Movie> | ApiError, ApiError, ApiResponse<Movie>>({
    queryKey: ['getNowPlayingMovies'],
    queryFn: () => movieClient.getNowPlayingMovies(),
  })
