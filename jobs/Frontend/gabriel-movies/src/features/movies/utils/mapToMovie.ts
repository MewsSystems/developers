import type { Movie, TmdbMovieDetailsResponse, TmdbSearchResponse } from "../types";
import { TMDB_IMAGE_BASE, TMDB_POSTER_SIZE } from "../constants";
import { formatDate } from "@/shared/utils/formatDate";

const getPosterUrl = (path: string): string => `${TMDB_IMAGE_BASE}/${TMDB_POSTER_SIZE}${path}`;

export const mapToMovie = (
  raw: TmdbSearchResponse["results"][number] | TmdbMovieDetailsResponse
): Movie => ({
  id: raw.id,
  title: raw.title,
  overview: raw.overview,
  releaseDate: raw.release_date,
  formattedReleaseDate: raw.release_date ? formatDate(raw.release_date) : "",
  popularity: raw.popularity,
  posterPath: getPosterUrl(raw.poster_path),
  voteAverage: raw.vote_average ?? 0,
});