import { useSuspenseInfiniteQuery } from '@tanstack/react-query';
import { FetchHttpRepository } from '@/modules/movies/infrastructure/FetchHttpRepository';
import { TMDBMovieRepository } from '@/modules/movies/infrastructure/TMDBMovieRepository';
import { searchMovie } from '@/modules/movies/application/searchMovie';

const httpRepository = new FetchHttpRepository();
const moviesRepository = new TMDBMovieRepository(httpRepository);

// 30 minutes to next refetch
const STALE_TIME = 1000 * 60 * 30;

const QUERY_IDENTIFIER = 'movies';
const INITIAL_PAGE = 1;

const useSearchMovie = (query: string) => {
  return useSuspenseInfiniteQuery({
    queryKey: [{ queryIdentifier: QUERY_IDENTIFIER, query }],
    queryFn: async ({ pageParam }) =>
      searchMovie(moviesRepository, query, pageParam),
    initialPageParam: INITIAL_PAGE,
    getNextPageParam: (lastPage) => {
      return lastPage.page < lastPage.totalPages
        ? lastPage.page + 1
        : undefined;
    },
    staleTime: STALE_TIME,
  });
};

export default useSearchMovie;
