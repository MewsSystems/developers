import authenticatedApiClient from '../auth/authenticatedApiClient';
import { Movie } from '../movies/models/movie';

export async function searchMovie(query: string) {
  return await authenticatedApiClient.get<Movie[]>(`/search/movie?query=${query}`);
}
