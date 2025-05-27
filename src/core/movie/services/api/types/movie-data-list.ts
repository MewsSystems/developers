import { Movie } from '@core/movie/types/movie';

export interface MoviesDataList {
  movies: Movie[];
  totalPages: number;
}
