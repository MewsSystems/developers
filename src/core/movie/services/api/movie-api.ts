import { createApiClient } from '@services/FetchRestClient';
import { RestClient } from '@services/types/rest-api';
import { getMoviesAdapter } from '../adapter/get-movies-adapter';
import { getMovieDetailsAdapter } from '../adapter/get-movie-adapter';
import { Movie } from '../../types/movie';

interface MovieResponse {
  results: Movie[];
  page: number;
  total_pages: number;
  total_results: number;
}

interface MoviesDataList {
  movies: Movie[];
  totalPages: number;
}

const movieApi: RestClient = createApiClient();

const isValidMoviesResponse = (data: unknown): data is MovieResponse => {
  return (
    typeof data === 'object' &&
    data !== null &&
    'results' in data &&
    Array.isArray((data as any).results) &&
    'page' in data &&
    'total_pages' in data &&
    'total_results' in data
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

export const getMovies = async (page: number = 1): Promise<MoviesDataList> => {
  try {
    const response = await movieApi.get({ 
      url: `/movie/popular?page=${page}` 
    });
    
    if (!isValidMoviesResponse(response)) {
      throw new Error('Invalid movies response');
    }
    
    return {
      movies: getMoviesAdapter(response.results),
      totalPages: response.total_pages
    };
  } catch (error) {
    console.error('Error fetching movies:', error);
    throw error;
  }
};  

export const searchMovies = async (query: string, page: number = 1): Promise<MoviesDataList> => {
  try {
    const response = await movieApi.get({ 
      url: `/search/movie?query=${encodeURIComponent(query)}&page=${page}` 
    });
    
    if (!isValidMoviesResponse(response)) {
      throw new Error('Invalid search response');
    }
    
    return {
      movies: getMoviesAdapter(response.results),
      totalPages: response.total_pages
    };
  } catch (error) {
    console.error('Error searching movies:', error);
    throw error;
  }
};

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