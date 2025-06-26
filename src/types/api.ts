import type { TMDBMovie, TMDBSearchResponse, TMDBMovieDetail } from './tmdb';

export type APIResponse<T> = T | { error: string };

export interface PosterUrl {
  default: string | null;
}

export type MovieSearchResponse = Omit<TMDBSearchResponse, 'results'> & {
  results: (TMDBMovie & {
    poster_url: PosterUrl;
  })[];
};

export type MovieDetailResponse = TMDBMovieDetail & {
  poster_url: { default: string | null };
};
