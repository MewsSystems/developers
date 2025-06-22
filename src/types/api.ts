import type { TMDBMovie, TMDBSearchResponse, TMDBMovieDetail } from './tmdb';

export type APIResponse<T> = T | { error: string };

export type MovieSearchResponse = Omit<TMDBSearchResponse, 'results'> & {
  results: (TMDBMovie & {
    poster_url: { default: string | null };
  })[];
};

export type MovieDetailResponse = TMDBMovieDetail & {
  poster_url: { default: string | null };
};
