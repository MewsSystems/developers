import { useQuery } from '@tanstack/react-query';
import { fetchMovieDetail } from '../services/moviesApi';

export const useMovieDetail = (id: number) => {
  return useQuery({
    queryKey: ['movie', id],
    queryFn: () => fetchMovieDetail(id),
    enabled: !!id,
  });
};
