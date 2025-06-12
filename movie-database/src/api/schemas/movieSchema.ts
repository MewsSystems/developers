import { z } from "zod";
import { extractYearFromDate } from "./utils";
import fallbackImage from "@/assets/poster-fallback.webp";

const IMAGE_BASE_URL = 'https://image.tmdb.org/t/p/w400';

export const movieResultSchema = z.object({
  id: z.number(),
  backdrop_path: z.string().nullable(),
  poster_path: z.string().nullable(),
  release_date: z.string(),
  title: z.string(),
  overview: z.string().min(10).catch(() => "No overview available"),
  vote_average: z.number(),
  vote_count: z.number(),
}).transform((data) => {
  const fetchedImage = data.backdrop_path ?? data.poster_path;
  return ({
    id: data.id,
    year: extractYearFromDate(data.release_date),
    title: data.title,
    overview: data.overview,
    voteAverage: data.vote_average.toFixed(1),
    voteCount: data.vote_count,
    image: fetchedImage !== null ? `${IMAGE_BASE_URL}${fetchedImage}` : fallbackImage,
  });
});

export const movieResultsListSchema = z.object({
  page: z.number(),
  results: z.array(movieResultSchema),
  total_pages: z.number(),
  total_results: z.number(),
}).transform((data) => ({
  page: data.page,
  results: data.results,
  totalPages: data.total_pages,
  totalResults: data.total_results,
}));

export const movieSchema = z.object({
  genres: z.array(
    z.object({
      id: z.number(),
      name: z.string(),
    })
  ),
  id: z.number(),
  overview: z.string().min(10).catch(() => "No overview available"),
  poster_path: z.string().nullable(),
  title: z.string(),
  vote_average: z.number(),
  vote_count: z.number(),
  origin_country: z.array(z.string()),
  release_date: z.string().transform((val) => extractYearFromDate(val)),
}).transform((data) => ({
  id: data.id,
  title: data.title,
  overview: data.overview,
  voteAverage: data.vote_average.toFixed(1),
  originCountries: data.origin_country,
  voteCount: data.vote_count,
  year: extractYearFromDate(data.release_date),
  genres: data.genres,
  image: data.poster_path !== null ? `${IMAGE_BASE_URL}${data.poster_path}` : fallbackImage,
}));

export type MovieResultsList = z.infer<typeof movieResultsListSchema>;
export type MovieResult = z.infer<typeof movieResultSchema>;
export type Movie = z.infer<typeof movieSchema>;
