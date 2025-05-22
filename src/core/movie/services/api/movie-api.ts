import { createApiClient } from '@services/FetchRestClient';
import { RestClient } from '@services/types/rest-api';
import { getMoviesAdapter } from '../adapter/get-movies-adapter';
import { Movie } from '../types/movie';

interface MovieResponse {
  results: Movie[];
  page: number;
  total_pages: number;
  total_results: number;
}

const movieApi: RestClient = createApiClient();

export const getMovies = async (): Promise<Movie[]> => {
  try {
    const response = await movieApi.get<MovieResponse>({
      url: '/movie/popular',
    });
    if (response.results) {
      const movies = getMoviesAdapter(response.results)
      return movies;
    }
    return [];
  } catch (error) {
    console.error('Error fetching movies:', error);
    throw error;
  }
};


