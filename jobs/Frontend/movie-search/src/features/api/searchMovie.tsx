import authenticatedApiClient from '../auth/authenticatedApiClient';
import { Movie } from '../movies/models/Movie';
import { ApiData } from './models/ApiData';

interface MovieData extends Omit<ApiData, 'results'> {
  results: Movie[];
}

/**
 * Function that searches for movies based on a query string.
 *
 * @param query - The search query string to use for the API request.
 * @returns An object containing movie data, including an array of {@link Movie} objects in the 'results' property.
 *
 * @throws Throws an error if the API request fails.
 */
export async function searchMovies(query: string, page?: number) {
  const pageParameter = page ? `&page=${page}` : '';
  return await authenticatedApiClient.get<MovieData>(`/search/movie?query=${query}${pageParameter}`);
}
