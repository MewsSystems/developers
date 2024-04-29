import { MovieList } from '../../../interfaces/movie.interface';

export interface MovieListContainerProps {
  searchTerm: string | null;
  page: number;
  onFetchMoviesSuccess: (movies: MovieList) => void;
  onFetchMoviesError: () => void;
}
