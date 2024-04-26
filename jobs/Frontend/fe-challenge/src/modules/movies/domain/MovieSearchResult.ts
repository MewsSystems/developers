import { Movie } from '@/modules/movies/domain/Movie';

export interface MovieSearchResult {
  page: number;
  results: Array<Movie>;
  totalPages: number;
  totalResults: number;
}
