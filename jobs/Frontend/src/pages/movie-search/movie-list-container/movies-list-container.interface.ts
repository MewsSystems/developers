import { MovieList } from '../../../shared/interfaces/movie.interface';

export interface MovieListContainerProps {
  searchTerm: string | null;
  page: number;
  total: number;
  onFetchMoviesSuccess: (movies: MovieList) => void;
  onFetchMoviesError: () => void;
}
