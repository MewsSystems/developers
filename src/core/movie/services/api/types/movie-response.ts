import { Movie } from "@core/movie/types/movie";

export interface MovieResponse {
    results: Movie[];
    page: number;
    total_pages: number;
    total_results: number;
  }