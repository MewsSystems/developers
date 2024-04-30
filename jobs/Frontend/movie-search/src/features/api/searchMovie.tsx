import authenticatedApiClient from '../auth/authenticatedApiClient';
import { ApiData } from '../common/models/ApiData';
import { Movie } from '../movies/models/Movie';

interface MovieData extends Omit<ApiData, 'results'> {
  results: Movie[];
}

export async function searchMovie(query: string) {
  return await authenticatedApiClient.get<MovieData>(`/search/movie?query=${query}`);
}
