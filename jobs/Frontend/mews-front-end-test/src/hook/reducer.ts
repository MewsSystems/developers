import { Movie } from '../api/sendRequest';

interface MovieState {
  movies: Movie[];
  searchQuery: string;
  page: number;
  numberOfPages: number;
}

const reducer = (
  currentState: MovieState,
  nextState:
    | Partial<MovieState>
    | ((currentState: MovieState) => Partial<MovieState>),
) => ({
  ...currentState,
  ...(typeof nextState === 'function' ? nextState(currentState) : nextState),
});

const initialMovieState: MovieState = {
  movies: [],
  searchQuery: '',
  page: 1,
  numberOfPages: 1,
};

export { reducer, initialMovieState };
