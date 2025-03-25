import type { z } from "zod";
import type {
  creditsSchema,
  movieDetailExtendedSchema,
  movieDetailsSchema,
  movieSchema,
  tmdbResponseSchema,
} from "~/schemas/movies.schemas";

export type Movie = z.infer<typeof movieSchema>;

export type MovieDetails = z.infer<typeof movieDetailsSchema>;

export type MovieDetailExtended = z.infer<typeof movieDetailExtendedSchema>;

export type MovieApiResponse = z.infer<typeof tmdbResponseSchema>;

export type Credits = z.infer<typeof creditsSchema>;
