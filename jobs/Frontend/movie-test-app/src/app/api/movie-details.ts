import { moviesApiClient } from './lib/api-client-movies.ts';
import { queryOptions } from '@tanstack/react-query';
import { MovieDetailsResponse } from '../../types/api.ts';

export const getMovieDetails = ({ movieId }: { movieId: string }): Promise<{ data: MovieDetailsResponse }> => {
  return moviesApiClient.get(`/movie/${movieId}`);
};

export const getMovieDetailsQueryOptions = (movieId: string) => {
  return queryOptions({
    queryKey: ['movieDetails', movieId],
    queryFn: () => getMovieDetails({ movieId }),
  });
};
