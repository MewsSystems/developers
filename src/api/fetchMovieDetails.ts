import type {Movie} from './types';
import {movieApiService} from './movieApiService';

export const fetchMovieDetails = async (movieId: string): Promise<Movie> => {
  return (await movieApiService.get(`/movie/${movieId}`)).data;
};
