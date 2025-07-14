import type { TMDBMovie, TMDBSearchResponse, TMDBMovieDetail } from './tmdb';
import type { UNUSED_MOVIE_SEARCH_KEYS, UNUSED_MOVIE_DETAIL_KEYS } from '@/lib/tmdbUtils';

export type APIResponse<T> = T | { error: string };

export interface PosterUrl {
  default: string | null;
  sm: string | null;
  md: string | null;
}

export interface DetailedPosterUrl {
  default: string | null;
  sm: string | null;
  lg: string | null;
}

export type StrippedMovieKeys = (typeof UNUSED_MOVIE_SEARCH_KEYS)[number];
export type StrippedMovieDetailKeys = (typeof UNUSED_MOVIE_DETAIL_KEYS)[number];

export type MovieSearchResult = Omit<TMDBMovie, StrippedMovieKeys> & {
  poster_url: PosterUrl;
};

export type MovieSearchResponse = Omit<TMDBSearchResponse, 'results'> & {
  results: MovieSearchResult[];
};

export type MovieDetailResponse = Omit<TMDBMovieDetail, StrippedMovieDetailKeys> & {
  poster_url: DetailedPosterUrl;
};
