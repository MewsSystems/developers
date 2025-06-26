import { MovieSearchResponse } from '@/types/api';

const REVALIDATE_TIME = parseInt(process.env.SEARCH_REVALIDATE_TIME || '300');

export async function fetchMovies(search: string, page = 1): Promise<MovieSearchResponse> {
  const res = await fetch(
    `${process.env.NEXT_PUBLIC_SITE_URL}/api/movies?search=${encodeURIComponent(search)}&page=${page}`,
    {
      next: { revalidate: REVALIDATE_TIME },
    }
  );

  if (!res.ok) {
    throw new Error('Failed to fetch movies (SSR)');
  }

  return res.json();
}
