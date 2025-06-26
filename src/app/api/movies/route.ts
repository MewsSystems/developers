import { NextRequest, NextResponse } from 'next/server';
import { z } from 'zod';
import { fetchTMDB } from '@/lib/tmdb';
import type { TMDBSearchResponse, TMDBMovie } from '@/types/tmdb';
import type { APIResponse, MovieSearchResponse } from '@/types/api';
import { enrichWithPosterUrl, getTMDBImageConfig } from '@/lib/tmdbUtils';
import { ApiError } from '@/lib/apiError';

const querySchema = z.object({
  search: z.string().min(1),
  page: z.coerce.number().int().positive().default(1),
});

export async function GET(
  req: NextRequest
): Promise<NextResponse<APIResponse<MovieSearchResponse>>> {
  try {
    const url = new URL(req.url);
    const query = Object.fromEntries(url.searchParams.entries());

    const result = querySchema.safeParse(query);

    if (!result.success) {
      throw new ApiError('Invalid query', 400);
    }

    const { search, page } = result.data;

    const revalidate = Number(process.env.MOVIES_SEARCH_REVALIDATE ?? '300');

    const moviesRes = await fetchTMDB(
      `/search/movie?query=${encodeURIComponent(search)}&page=${page}`,
      revalidate
    );

    if (!moviesRes.ok) {
      throw new ApiError('unable to get movies', moviesRes.status);
    }

    const data: TMDBSearchResponse = await moviesRes.json();

    const { base, poster_sizes } = await getTMDBImageConfig();

    const size = poster_sizes.includes('w154') ? 'w154' : '';

    const enrichedResults = enrichWithPosterUrl<TMDBMovie>(data.results, base, size);

    return NextResponse.json({ ...data, results: enrichedResults }, { status: 200 });
  } catch (e: unknown) {
    const error = e instanceof ApiError ? e.message : 'unable to get movies';
    const status = e instanceof ApiError ? e.status : 500;
    return NextResponse.json({ error }, { status });
  }
}
