import { NextRequest, NextResponse } from 'next/server';
import { z } from 'zod';
import { fetchTMDB } from '@/lib/tmdb';
import type { TMDBSearchResponse, TMDBConfigurationResponse } from '@/types/tmdb';
import type { APIResponse, MovieSearchResponse } from '@/types/api';

const querySchema = z.object({
  search: z.string().min(1),
  page: z.coerce.number().int().positive().default(1),
});

export async function GET(
  req: NextRequest
): Promise<NextResponse<APIResponse<MovieSearchResponse>>> {
  const url = new URL(req.url);
  const query = Object.fromEntries(url.searchParams.entries());

  const result = querySchema.safeParse(query);

  if (!result.success) {
    return NextResponse.json({ error: 'Invalid query' }, { status: 400 });
  }

  const { search, page } = result.data;

  const revalidate = Number(process.env.MOVIES_SEARCH_REVALIDATE ?? '300');
  const configRevalidate = Number(process.env.MOVIES_CONFIGURATION_REVALIDATE ?? '7200');

  const movieRes = await fetchTMDB(
    `/search/movie?query=${encodeURIComponent(search)}&page=${page}`,
    revalidate
  );

  if (!movieRes.ok) {
    return NextResponse.json({ error: 'unable to get movies' }, { status: movieRes.status });
  }

  const data: TMDBSearchResponse = await movieRes.json();

  const configRes = await fetchTMDB('/configuration', configRevalidate);

  if (!configRes.ok) {
    return NextResponse.json({ error: 'unable to get movies' }, { status: 500 });
  }

  const config: TMDBConfigurationResponse = await configRes.json();
  const base = config.images.secure_base_url;
  const size = config.images.poster_sizes.includes('w154') ? 'w154' : '';

  const enrichedResults = data.results.map((movie) => ({
    ...movie,
    poster_url: {
      default: movie.poster_path ? `${base}${size}${movie.poster_path}` : null,
    },
  }));

  return NextResponse.json({ ...data, results: enrichedResults }, { status: 200 });
}
