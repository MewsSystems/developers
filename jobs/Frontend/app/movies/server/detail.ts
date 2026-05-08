import { createServerFn } from '@tanstack/start';
import * as v from 'valibot';
import { moviesMiddleware } from './middleware';
import { queryOptions } from '@tanstack/react-query';

export interface Genre {
  readonly id: number;
  readonly name: string;
}

export interface Country {
  readonly name: string;
  readonly iso_3166_1: string;
}

export interface MovieDetail {
  adult: boolean;
  backdrop_path: string | null;
  belongs_to_collection: string | null;
  budget: number;
  genres: ReadonlyArray<Genre>;
  homepage: string;
  id: number;
  imdb_id: string;
  origin_country: ReadonlyArray<'VE'>;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  production_companies: ReadonlyArray<string>;
  production_countries: ReadonlyArray<Country>;
  release_date: string;
  revenue: number;
  runtime: number;
  spoken_languages: ReadonlyArray<string>;
  status: string;
  tagline: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export const getMovieDetail = createServerFn()
  .middleware([moviesMiddleware])
  .validator(
    v.object({
      detail: v.number(),
    }),
  )
  .handler(async ({ context, data }): Promise<MovieDetail> => {
    const response = await fetch(
      `${context.moviesUrl}/movie/${data.detail}?api_key=${context.apiKey}`,
    );
    return await response.json();
  });

export const movieDetailQueryOptions = (movieId: number) =>
  queryOptions({
    queryKey: ['movie', movieId],
    queryFn: async () => await getMovieDetail({ data: { detail: movieId } }),
  });
