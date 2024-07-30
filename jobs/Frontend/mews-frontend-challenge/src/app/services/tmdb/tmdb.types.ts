// tmdb-ts is a great library but only allows auth via accessToken, not apiKey
// Due to this, we reuse the typings and re-implement their api methods since we only use a few of them
// TODO: Request a PR to use apiKey in tmdb-ts
import {
  MovieDetails as _MovieDetails,
  Credits,
  Cast,
  Crew,
  Genre,
  Movie as MovieResult,
} from "tmdb-ts";

export type MovieDetails = _MovieDetails & {
  // For some reason, none of the SDKs I've tested have this data
  origin_country?: string[];
};

export type { Credits, Cast, Crew, Genre, MovieResult };
