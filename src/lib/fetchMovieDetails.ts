import { MovieDetailResponse } from '@/types/api';

const REVALIDATE_TIME = parseInt(process.env.MOVIE_REVALIDATE_TIME || '300');

export async function fetchMovieDetails(movieId: string): Promise<MovieDetailResponse> {
  const res = await fetch(`${process.env.NEXT_PUBLIC_SITE_URL}/api/movies/${movieId}`, {
    next: { revalidate: REVALIDATE_TIME },
  });

  if (!res.ok) {
    throw new Error(`Failed to fetch movie details (SSR) for movieId: ${movieId}`);
  }

  return res.json();
}
