import type { Movie } from '../types/movieTypes';

export type ListMoviesResponse = {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
};

export type ListMoviesParams = {
  query: string;
  page?: number;
};

export type ErrorApiResponse = {
  status_code: number;
  status_message: string;
  success: boolean;
};

