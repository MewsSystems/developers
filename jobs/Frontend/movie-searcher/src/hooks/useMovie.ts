import { useQuery } from '@tanstack/react-query';
import { getMovie } from '../services/movieService';

export const useMovie = (movieId?: string) => {
  return useQuery({
    queryKey: ['movies', movieId],
    enabled: !!movieId,
    queryFn: async () => await getMovie(movieId!),
  });
};
