import * as z from 'zod'

export const GenreSchema = z.object({
  id: z.number(),
  name: z.string(),
})
export type Genre = z.infer<typeof GenreSchema>

export const MovieDetailSchema = z.object({
  adult: z.boolean(),
  backdrop_path: z.string().nullable(),
  belongs_to_collection: z.null(),
  budget: z.number(),
  genres: z.array(GenreSchema),
  homepage: z.string(),
  id: z.number(),
  imdb_id: z.string(),
  origin_country: z.array(z.string()),
  original_language: z.string(),
  original_title: z.string(),
  overview: z.string(),
  popularity: z.number(),
  poster_path: z.string(),
  production_companies: z.array(z.any()),
  production_countries: z.array(z.any()),
  release_date: z.string(),
  revenue: z.number(),
  runtime: z.number(),
  spoken_languages: z.array(z.any()),
  status: z.string(),
  tagline: z.string(),
  title: z.string(),
  video: z.boolean(),
  vote_average: z.number(),
  vote_count: z.number(),
})
export type MovieDetail = z.infer<typeof MovieDetailSchema>
