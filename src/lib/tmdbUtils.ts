import { fetchTMDB } from '@/lib/fetch/fetchTMDB';
import type { TMDBConfigurationResponse, TMDBMovie, TMDBMovieDetail } from '@/types/tmdb';

const revalidate = Number(process.env.MOVIES_CONFIGURATION_REVALIDATE ?? '7200');

export async function getTMDBImageConfig(): Promise<{ base: string; poster_sizes: string[] }> {
  const configRes = await fetchTMDB('/configuration', revalidate);

  if (!configRes.ok) {
    throw new Error('tmdb config fetch failed');
  }

  const config: TMDBConfigurationResponse = await configRes.json();
  const base = config.images.secure_base_url;
  const poster_sizes = config.images.poster_sizes;

  return { base, poster_sizes };
}

type SizeMap = { [K in string]: string | undefined };

export function enrichWithPosterUrl<T extends { poster_path: string | null }, S extends SizeMap>(
  items: T[],
  base: string,
  sizes: S
): (T & {
  poster_url: { [K in keyof S]: string | null };
})[] {
  return items.map((item) => {
    const poster_url = Object.fromEntries(
      Object.entries(sizes).map(([key, size]) => [
        key,
        item.poster_path && base && size ? `${base}${size}${item.poster_path}` : null,
      ])
    ) as { [K in keyof S]: string | null };

    return {
      ...item,
      poster_url,
    };
  });
}

export const UNUSED_MOVIE_DETAIL_KEYS = [
  'adult',
  'backdrop_path',
  'belongs_to_collection',
  'budget',
  'homepage',
  'imdb_id',
  'popularity',
  'poster_path',
  'revenue',
  'video',
] as const satisfies readonly (keyof TMDBMovieDetail)[];

export const UNUSED_MOVIE_SEARCH_KEYS = [
  'adult',
  'backdrop_path',
  'genre_ids',
  'popularity',
  'video',
] as const satisfies readonly (keyof TMDBMovie)[];

export function stripKeysFromObject<T extends object, K extends keyof T>(
  obj: T,
  keys: readonly K[]
): Omit<T, K> {
  const copy: Partial<T> = { ...obj };
  for (const key of keys) {
    if (key in copy) {
      delete copy[key];
    }
  }
  return copy as Omit<T, K>;
}

export function stripKeysFromArray<T extends object, K extends keyof T>(
  arr: T[],
  keys: readonly K[]
): Omit<T, K>[] {
  return arr.map((item) => stripKeysFromObject(item, keys));
}
