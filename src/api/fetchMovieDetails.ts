import type {Movie} from './types.ts';
import {movieApiService} from './movieApiService.ts';

export const fetchMovieDetails = async (movieId: string): Promise<Movie> => {
  return (await movieApiService.get(`/movie/${movieId}`)).data;
};
