import { useQuery } from '@tanstack/react-query'
import movieClient from '../../api/movieClient/movieClient'

export const useGetMovieDetails = (movieId: string) =>
  useQuery({
    queryKey: ['getMovieDetails'],
    queryFn: () => movieClient.getMovieDetails(movieId),
  })
