import authenticatedApiClient from '../auth/authenticatedApiClient';
import { Details } from '../movies/models/Details';

/**
 * Function that retrieves detailed information for a specific movie.
 *
 * @param id - The ID of the movie to retrieve details for.
 * @returns An object containing detailed movie information of type {@link Details}.
 *
 * @throws Throws an error if the API request fails.
 */
export async function getMovieDetails(id: string) {
  return await authenticatedApiClient.get<Details>(`/movie/${id}`);
}
