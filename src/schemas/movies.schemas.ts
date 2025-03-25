import { z } from "zod";

export const getPopularMovesInputSchema = z.object({
  page: z.number().min(1).max(500).optional(),
});

export const searchMoviesInputSchema = getPopularMovesInputSchema.extend({
  query: z.string().min(1),
});

export const movieSchema = z.object({
  id: z.number(),
  title: z.string(),
  poster_path: z.string().nullable(),
  overview: z.string(),
  release_date: z.string(),
  adult: z.boolean(),
  backdrop_path: z.string().nullable(),
  genre_ids: z.array(z.number()),
  original_language: z.string(),
  original_title: z.string(),
  popularity: z.number(),
  video: z.boolean(),
  vote_average: z.number(),
  vote_count: z.number(),
});

export const tmdbResponseSchema = z.object({
  page: z.number(),
  results: z.array(movieSchema),
  total_pages: z.number(),
  total_results: z.number(),
});

export const movieDetailsSchema = z.object({
  adult: z.boolean(),
  backdrop_path: z.string().nullable(),
  belongs_to_collection: z
    .object({
      id: z.number(),
      name: z.string(),
      poster_path: z.string().nullable(),
      backdrop_path: z.string().nullable(),
    })
    .nullable(),
  budget: z.number(),
  genres: z.array(z.object({ id: z.number(), name: z.string() })),
  homepage: z.string().nullable(),
  id: z.number(),
  imdb_id: z.string().nullable(),
  original_language: z.string(),
  original_title: z.string(),
  overview: z.string().nullable(),
  popularity: z.number(),
  poster_path: z.string().nullable(),
  production_companies: z.array(
    z.object({
      id: z.number(),
      logo_path: z.string().nullable(),
      name: z.string(),
      origin_country: z.string(),
    }),
  ),
  production_countries: z.array(
    z.object({
      iso_3166_1: z.string(),
      name: z.string(),
    }),
  ),
  release_date: z.string(),
  revenue: z.number(),
  runtime: z.number().nullable(),
  spoken_languages: z.array(
    z.object({
      iso_639_1: z.string(),
      name: z.string(),
    }),
  ),
  status: z.string(),
  tagline: z.string().nullable(),
  title: z.string(),
  video: z.boolean(),
  vote_average: z.number(),
  vote_count: z.number(),
});
export const castMemberSchema = z.object({
  id: z.number(),
  name: z.string(),
  character: z.string(),
  profile_path: z.string().nullable(),
});

export const movieDetailExtendedSchema = movieDetailsSchema.extend({
  director: z.object({ id: z.number(), name: z.string() }).nullable(),
  cast: z.array(castMemberSchema),
});

export const crewMemberSchema = z.object({
  id: z.number(),
  name: z.string(),
  job: z.string(),
  department: z.string(),
});

export const creditsSchema = z.object({
  id: z.number(),
  cast: z.array(castMemberSchema),
  crew: z.array(crewMemberSchema),
});
