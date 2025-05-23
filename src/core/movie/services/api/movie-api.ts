import { createApiClient } from '@services/FetchRestClient';
import { RestClient } from '@services/types/rest-api';
import { getMoviesAdapter } from '../adapter/get-movies-adapter';
import { getMovieDetailsAdapter } from '../adapter/get-movie-adapter';
import { Movie } from '../types/movie';

const movieApi: RestClient = createApiClient();

const isValidMoviesResponse = (data: unknown): data is { results: any[] } => {
  return (
    typeof data === 'object' &&
    data !== null &&
    'results' in data &&
    Array.isArray((data as any).results)
  );
}

const isValidMovieResponse = (data: unknown): data is { id: number } => {
  return (
    typeof data === 'object' &&
    data !== null &&
    'id' in data &&
    typeof (data as any).id === 'number'
  );
}

export const getMovies = async (): Promise<Movie[]> => {
  try {
    const response = await movieApi.get({ url: '/movie/popular' });
    if (!isValidMoviesResponse(response)) {
      throw new Error('Invalid movies response');
    }
    return getMoviesAdapter(response.results);
  } catch (error) {
    console.error('Error fetching movies:', error);
    throw error;
  }
};

export const searchMovies = async (query: string): Promise<Movie[]> => {
  try {
    const response = await movieApi.get({ url: `/search/movie?query=${encodeURIComponent(query)}` });
    if (!isValidMoviesResponse(response)) {
      throw new Error('Invalid search response');
    }
    return getMoviesAdapter(response.results);
  } catch (error) {
    console.error('Error searching movies:', error);
    throw error;
  }
};

export const getMovieDetails = async (movieId: number): Promise<Movie | null> => {
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