import { MovieDetailResponse } from '@/types/api';

export async function fetchMovieDetailsClient(movieId: string): Promise<MovieDetailResponse> {
  const res = await fetch(`/api/movies/${movieId}`, {
    cache: 'no-store',
  });

  if (!res.ok) {
    throw new Error(`Failed to fetch movie details (Client) for movieId: ${movieId}`);
  }

  return res.json();
}
