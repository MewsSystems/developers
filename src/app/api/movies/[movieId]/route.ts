import { NextRequest, NextResponse } from 'next/server';
import { z } from 'zod';
import { fetchTMDB } from '@/lib/fetch/api/fetchTMDB';
import type { TMDBMovieDetail } from '@/types/tmdb';
import type { APIResponse, MovieDetailResponse } from '@/types/api';
import { ApiError } from '@/lib/apiError';
import {
  enrichWithPosterUrl,
  getTMDBImageConfig,
  stripKeysFromObject,
  UNUSED_MOVIE_DETAIL_KEYS,
} from '@/lib/tmdbUtils';

const paramsSchema = z.object({
  movieId: z.string().regex(/^\d+$/, 'movieId must be a number'),
});

export async function GET(
  _req: NextRequest,
  context: { params: Promise<{ movieId: string }> }
): Promise<NextResponse<APIResponse<MovieDetailResponse>>> {
  try {
    const params = await context.params;

    if (!params?.movieId) {
      throw new ApiError('movieId param missing', 400);
    }

    const parsed = paramsSchema.safeParse({ movieId: params.movieId });

    if (!parsed.success) {
      throw new ApiError('Invalid movieId', 400);
    }

    const { movieId } = parsed.data;

    const revalidate = Number(process.env.MOVIES_DETAIL_REVALIDATE ?? '3600');

    const movieRes = await fetchTMDB(`/movie/${movieId}`, revalidate);

    if (!movieRes.ok) {
      throw new ApiError('unable to get movie', movieRes.status);
    }

    const movie: TMDBMovieDetail = await movieRes.json();

    const { base, poster_sizes } = await getTMDBImageConfig();

    const sizes = {
      default: poster_sizes.includes('w185') ? 'w185' : undefined,
      sm: poster_sizes.includes('w342') ? 'w342' : undefined,
      lg: poster_sizes.includes('w500') ? 'w500' : undefined,
    };

    const [enrichedMovie] = enrichWithPosterUrl<TMDBMovieDetail, typeof sizes>(
      [movie],
      base,
      sizes
    );

    const cleanedMovie = stripKeysFromObject(enrichedMovie, UNUSED_MOVIE_DETAIL_KEYS);

    return NextResponse.json<MovieDetailResponse>(cleanedMovie, { status: 200 });
  } catch (e: unknown) {
    const error = e instanceof ApiError ? e.message : 'unable to get movie';
    const status = e instanceof ApiError ? e.status : 500;

    return NextResponse.json({ error }, { status });
  }
}
