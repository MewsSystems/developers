import { useQuery } from '@tanstack/react-query';
import { fetchPopularMovies } from '../api/fetch';

export const useGetPopularMovies = () => {
  return useQuery({
    queryKey: ['popularMovies'],
    queryFn: () => fetchPopularMovies(),
  });
};
