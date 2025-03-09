import * as z from 'zod'

export const MovieResultSchema = z.object({
  adult: z.boolean().default(true),
  backdrop_path: z.string().nullable(),
  genre_ids: z.array(z.number()),
  id: z.number().default(0),
  original_language: z.string(),
  original_title: z.string(),
  overview: z.string().nullable(),
  popularity: z.number().default(0),
  poster_path: z.string().nullable(),
  release_date: z.string().nullable(),
  title: z.string(),
  video: z.boolean().default(true),
  vote_average: z.number().default(0),
  vote_count: z.number().default(0),
})

export const MovieSearchResultSchema = z.object({
  page: z.number().default(0),
  results: z.array(MovieResultSchema),
  total_pages: z.number().default(0),
  total_results: z.number().default(0),
})

export type MovieResult = z.infer<typeof MovieResultSchema>
export type MovieSearchResult = z.infer<typeof MovieSearchResultSchema>
