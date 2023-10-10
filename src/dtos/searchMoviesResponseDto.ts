import { MovieDto } from "./movieDto";

export interface SearchMoviesResponseDto {
  page: number;
  results: MovieDto[];
  total_pages: number;
  total_results: number;
}
