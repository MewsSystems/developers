import { useQuery } from '@tanstack/react-query';
import { fetchDetailsMovie } from '../api/fetch';

export const useGetDetailsMovie = (movieId = '') => {
  return useQuery({
    queryKey: ['detailsMovie', movieId],
    queryFn: () => fetchDetailsMovie(movieId),
    enabled: !!movieId,
  });
};
