import { api } from './lib/api-client';
import { queryOptions, useQuery } from '@tanstack/react-query';
import { MovieDetailsResponse } from '../../types/api.ts';
import { QueryConfig } from './lib/react-query-config.ts';

export const getMovieDetails = ({ movieId }: { movieId: string }): Promise<{ data: MovieDetailsResponse }> => {
  return api.get(`/movie/${movieId}`);
};

export const getMovieDetailsQueryOptions = (movieId: string) => {
  return queryOptions({
    queryKey: ['movieDetails', movieId],
    queryFn: () => getMovieDetails({ movieId }),
  });
};

type UseMovieDetailsOptions = {
  movieId: string;
  queryConfig?: QueryConfig<typeof getMovieDetailsQueryOptions>;
};

export const useMovieDetails = ({ movieId, queryConfig }: UseMovieDetailsOptions) => {
  return useQuery({
    ...getMovieDetailsQueryOptions(movieId),
    ...queryConfig,
  });
};
