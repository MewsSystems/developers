import { useSuspenseQuery } from '@tanstack/react-query';
import { FetchHttpRepository } from '@/modules/movies/infrastructure/FetchHttpRepository';
import { TMDBMovieRepository } from '@/modules/movies/infrastructure/TMDBMovieRepository';
import { getMovieDetail } from '@/modules/movies/application/getMovieDetail';

const httpRepository = new FetchHttpRepository();
const moviesRepository = new TMDBMovieRepository(httpRepository);

// 30 minutes to next refetch
export const MOVIE_DETAIL_STALE_TIME = 1000 * 60 * 30;

const QUERY_IDENTIFIER = 'movie';

export const buildMovieDetailQueryKey = (id: number) => [
  { queryIdentifier: QUERY_IDENTIFIER, id },
];

const useGetMovieDetail = (id: number) => {
  return useSuspenseQuery({
    queryKey: buildMovieDetailQueryKey(id),
    queryFn: async () => getMovieDetail(moviesRepository, id),
    staleTime: MOVIE_DETAIL_STALE_TIME,
  });
};

export default useGetMovieDetail;
