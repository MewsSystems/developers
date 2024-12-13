import { useQuery } from '@tanstack/react-query'
import movieClient from '../../api/movieClient/movieClient'
import {
  FullMovieResponse,
  ApiError,
} from '../../api/movieClient/movieClientTypes'

export const useGetMovieDetails = (movieId: string) =>
  useQuery<FullMovieResponse | ApiError, ApiError, FullMovieResponse>({
    queryKey: ['getMovieDetails', movieId],
    queryFn: () => movieClient.getMovieDetails(movieId),
    enabled: !!movieId,
  })
