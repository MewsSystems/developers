import { useQueryClient } from '@tanstack/react-query';
import { FetchHttpRepository } from '@/modules/movies/infrastructure/FetchHttpRepository';
import { TMDBMovieRepository } from '@/modules/movies/infrastructure/TMDBMovieRepository';
import { getMovieDetail } from '@/modules/movies/application/getMovieDetail';
import {
  MOVIE_DETAIL_STALE_TIME,
  buildMovieDetailQueryKey,
} from '@/pages/movie-detail/hooks/useGetMovieDetail';

const httpRepository = new FetchHttpRepository();
const moviesRepository = new TMDBMovieRepository(httpRepository);

const usePrefetchMovieDetail = () => {
  const queryClient = useQueryClient();

  const prefetch = (id: number) => {
    queryClient.prefetchQuery({
      queryKey: buildMovieDetailQueryKey(id),
      queryFn: async () => getMovieDetail(moviesRepository, id),
      staleTime: MOVIE_DETAIL_STALE_TIME,
    });
  };

  return { prefetch };
};

export default usePrefetchMovieDetail;
