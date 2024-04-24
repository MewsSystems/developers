import { useCallback, useEffect, useReducer } from 'react';
import { Movie, MovieApiResponse, sendRequest } from '../api/sendRequest';

export interface UseMovies {
  movies: Movie[];
  searchQuery: string;
  setSearchQuery: (query: string) => void;
  page: number;
  numberOfPages: number;
  incrementPageNumber: () => void;
  decrementPageNumber: () => void;
}

interface MovieState {
  movies: Movie[];
  searchQuery: string;
  page: number;
  numberOfPages: number;
}

const movieReducer = (
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

const useMovies = (): UseMovies => {
  const [moviesState, dispatch] = useReducer(movieReducer, initialMovieState);

  const { movies, searchQuery, page, numberOfPages } = moviesState;

  const incrementPageNumber = () => {
    dispatch((previous) => {
      if (previous.page === numberOfPages) {
        return previous;
      }

      return { ...previous, page: previous.page + 1 };
    });
  };

  const decrementPageNumber = () => {
    dispatch((previous) => {
      if (previous.page === 1) {
        return previous;
      }

      return { ...previous, page: previous.page - 1 };
    });
  };

  const setSearchQuery = (query: string) => {
    dispatch({ searchQuery: query });
  };

  const sendMovieRequest = useCallback(sendRequest, [sendRequest]);

  useEffect(() => {
    if (Boolean(searchQuery)) {
      sendMovieRequest(searchQuery, page)
        .then((response: MovieApiResponse) => {
          dispatch({
            movies: response.results,
            numberOfPages: response.total_pages,
          });

          console.log('response: ', response);
        })
        .catch((error) => {
          console.error(error);
        });
    }
  }, [searchQuery, page, sendMovieRequest]);

  return {
    movies,
    searchQuery,
    setSearchQuery,
    numberOfPages,
    page,
    incrementPageNumber,
    decrementPageNumber,
  };
};

export { useMovies };
