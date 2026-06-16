import { operations } from "../types/api";

export type SearchMovieQuery = operations["search-movie"]["parameters"]["query"];
export type SearchMovieResponse = operations["search-movie"]["responses"][200]["content"]["application/json"];

export type MovieDetailsResponse = operations["movie-details"]["responses"][200]["content"]["application/json"];

export type Movie = {
  id?: number;
  title?: string;
  path?: string;
}

export type ExtendedMovie = Movie & {
  overview?: string;
  releaseDate?: string;
  originalLanguage?: string;
}
