import { useQuery } from '@tanstack/react-query';
import { getMovies } from '../services/movieService';

export const useMovies = (search: string) => {
  return useQuery({
    queryKey: ['movies', search],
    queryFn: async () => await getMovies(search),
  });
};
