import authenticatedApiClient from '../auth/authenticatedApiClient';
import { Details } from '../movies/models/Details';

export async function getMovieDetails(id: string) {
  return await authenticatedApiClient.get<Details>(`/movie/${id}`);
}
