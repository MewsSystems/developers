import { isValidMovieResponse } from './validation';
import { Movie } from '@core/movie/types/movie';
import { movieApi } from './movie-api';
import { getMovieDetailsAdapter } from '../adapter/get-movie-adapter';

export const getMovie = async (movieId: number): Promise<Movie | null> => {
  try {
    const response = await movieApi.get({ url: `/movie/${movieId}` });
    if (!isValidMovieResponse(response)) {
      return null;
    }
    return getMovieDetailsAdapter(response);
  } catch (error) {
    console.error('Error fetching movie details:', error);
    throw error;
  }
};
