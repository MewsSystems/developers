import { useInfiniteQuery } from '@tanstack/react-query';
import { getMovies } from '../services/movieService';

export const useMoviesInfinite = (search: string) => {
  return useInfiniteQuery({
    queryKey: ['movies', search],
    queryFn: async (pageParam) => await getMovies(search, pageParam.pageParam),
    initialPageParam: 1,
    getNextPageParam: (lastPage) => {
      return lastPage.totalPages < lastPage.page ? undefined : lastPage.page + 1;
    },
  });
};
