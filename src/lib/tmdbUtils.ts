import { fetchTMDB } from '@/lib/tmdb';
import type { TMDBConfigurationResponse } from '@/types/tmdb';
import { PosterUrl } from '@/types/api';

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

export function enrichWithPosterUrl<T extends { poster_path: string | null }>(
  items: T[],
  base: string,
  size: string
): (T & { poster_url: PosterUrl })[] {
  return items.map((item) => ({
    ...item,
    poster_url: {
      default: item.poster_path && base && size ? `${base}${size}${item.poster_path}` : null,
    },
  }));
}
