import type {ERRORS_BY_HTTP_STATUS} from './constants';

export type Movie = {
  id: number;
  title: string;
  overview: string;
  poster_path: string | null;
  release_date: string;
  vote_average: number;
  runtime: number;
  tagline: string;
  genres: {
    id: number;
    name: string;
  }[];
};

export type MovieSearchResponse = {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
};

export type ErrorStatus = keyof typeof ERRORS_BY_HTTP_STATUS;
export type ErrorMessages<T extends ErrorStatus> = keyof (typeof ERRORS_BY_HTTP_STATUS)[T];

export type ApiErrorResponseDetails = {
  status: number;
  message: string;
};
