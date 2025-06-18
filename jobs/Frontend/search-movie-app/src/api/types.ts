import type { Movie } from '../types/movieTypes';

type ListMoviesResponse = {
  page: number;
  results: Movie[];
  total_pages: number;
  total_results: number;
};

type ListMoviesParams = {
  query: string;
  page?: number;
};

type ErrorApiResponse = {
  status_code: number;
  status_message: string;
  success: boolean;
};

export type { ErrorApiResponse, ListMoviesParams, ListMoviesResponse };
