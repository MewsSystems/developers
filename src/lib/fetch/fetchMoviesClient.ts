import { MovieSearchResponse } from '@/types/api';

export async function fetchMoviesClient(search: string, page = 1): Promise<MovieSearchResponse> {
  const res = await fetch(`/api/movies?search=${encodeURIComponent(search)}&page=${page}`, {
    cache: 'no-store',
  });

  if (!res.ok) {
    throw new Error('Failed to fetch movies (Client)');
  }

  return res.json();
}
