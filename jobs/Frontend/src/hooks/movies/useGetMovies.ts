import { useQuery } from '@tanstack/react-query'
import movieClient from '../../api/movieClient/movieClient'

export const useGetMovies = (query: string, page: number = 1) =>
  useQuery({
    queryKey: ['getMovies'],
    queryFn: () => movieClient.getMovies(query, page),
  })
