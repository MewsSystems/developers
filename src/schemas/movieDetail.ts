import * as z from 'zod'

const genreSchema = z.object({
  id: z.number().default(0),
  name: z.string(),
})

const productionCompanySchema = z.object({
  id: z.number().default(0),
  logo_path: z.string().nullable(),
  name: z.string(),
  origin_country: z.string(),
})

const productionCountrySchema = z.object({
  iso_3166_1: z.string(),
  name: z.string(),
})

const spokenLanguageSchema = z.object({
  english_name: z.string(),
  iso_639_1: z.string(),
  name: z.string(),
})

const belongsToCollectionSchema = z.object({
  backdrop_path: z.string().nullable(),
  id: z.number().default(0),
  name: z.string(),
  poster_path: z.string().nullable(),
})

export const MovieDetailSchema = z.object({
  adult: z.boolean().default(true),
  backdrop_path: z.string().nullable(),
  belongs_to_collection: belongsToCollectionSchema.nullable(),
  budget: z.number().default(0),
  genres: z.array(genreSchema),
  homepage: z.string().nullable(),
  id: z.number().default(0),
  imdb_id: z.string().nullable(),
  original_language: z.string(),
  original_title: z.string(),
  overview: z.string().nullable(),
  popularity: z.number().default(0),
  poster_path: z.string().nullable(),
  production_companies: z.array(productionCompanySchema),
  production_countries: z.array(productionCountrySchema),
  release_date: z.string().nullable(),
  revenue: z.number().default(0),
  runtime: z.number().nullable(),
  spoken_languages: z.array(spokenLanguageSchema),
  status: z.string().nullable(),
  tagline: z.string().nullable(),
  title: z.string(),
  video: z.boolean().default(true),
  vote_average: z.number().default(0),
  vote_count: z.number().default(0),
})

export type MovieDetail = z.infer<typeof MovieDetailSchema>
