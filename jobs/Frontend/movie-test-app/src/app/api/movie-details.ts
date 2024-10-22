import { api } from './lib/api-client';
import { queryOptions } from '@tanstack/react-query';
import { MovieDetailsResponse } from '../../types/api.ts';

export const getMovieDetails = ({ movieId }: { movieId: string }): Promise<{ data: MovieDetailsResponse }> => {
  return api.get(`/movie/${movieId}`);
};

export const getMovieDetailsQueryOptions = (movieId: string) => {
  return queryOptions({
    queryKey: ['movieDetails', movieId],
    queryFn: () => getMovieDetails({ movieId }),
  });
};
