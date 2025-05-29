import type {MovieSearchResponse} from './types.ts';
import {movieApiService} from './movieApiService.ts';

export const fetchMoviesList = async (
  query: string,
  page: number = 1,
): Promise<MovieSearchResponse> => {
  return (
    await movieApiService.get('/search/movie', {
      params: {
        query,
        page,
      },
    })
  ).data;
};
