import { useInfiniteQuery } from '@tanstack/react-query';
import { fetchMovies } from '../services/moviesApi';

export const useMoviesSearch = (query: string, initialPage: number = 1) => {
  return useInfiniteQuery({
    queryKey: ['movies', query],
    queryFn: async ({ pageParam = initialPage }) => fetchMovies(query, pageParam),
    initialPageParam: initialPage,
    enabled: !!query,
    getNextPageParam: (lastPage) => {
      if (lastPage.page < lastPage.total_pages) {
        return lastPage.page + 1;
      }
      return undefined;
    },
  });
};
