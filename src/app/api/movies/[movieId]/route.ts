import { NextRequest, NextResponse } from 'next/server';
import { z } from 'zod';
import { fetchTMDB } from '@/lib/tmdb';
import type { TMDBMovieDetail, TMDBConfigurationResponse } from '@/types/tmdb';
import type { APIResponse, MovieDetailResponse } from '@/types/api';

const paramsSchema = z.object({
  movieId: z.string().regex(/^\d+$/, 'movieId must be a number'),
});

export async function GET(
  _req: NextRequest,
  context: { params: Promise<{ movieId: string }> }
): Promise<NextResponse<APIResponse<MovieDetailResponse>>> {
  const params = await context.params;

  if (!params?.movieId) {
    return NextResponse.json({ error: 'movieId param missing' }, { status: 400 });
  }

  const parsed = paramsSchema.safeParse({ movieId: params.movieId });

  if (!parsed.success) {
    return NextResponse.json({ error: 'Invalid movieId' }, { status: 400 });
  }

  const { movieId } = parsed.data;

  const revalidate = Number(process.env.MOVIES_DETAIL_REVALIDATE ?? '3600');
  const configRevalidate = Number(process.env.MOVIES_CONFIGURATION_REVALIDATE ?? '7200');

  const movieRes = await fetchTMDB(`/movie/${movieId}`, revalidate);

  if (!movieRes.ok) {
    return NextResponse.json({ error: 'unable to get movies' }, { status: movieRes.status });
  }

  const movie: TMDBMovieDetail = await movieRes.json();

  const configRes = await fetchTMDB('/configuration', configRevalidate);

  if (!configRes.ok) {
    return NextResponse.json({ error: 'unable to get movies' }, { status: 500 });
  }

  const config: TMDBConfigurationResponse = await configRes.json();
  const base = config.images.secure_base_url;
  const size = config.images.poster_sizes.includes('w342') ? 'w342' : '';

  const enrichedMovie: MovieDetailResponse = {
    ...movie,
    poster_url: {
      default: movie.poster_path ? `${base}${size}${movie.poster_path}` : null,
    },
  };

  return NextResponse.json(enrichedMovie, { status: 200 });
}
