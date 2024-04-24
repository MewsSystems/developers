import { useCallback, useEffect, useReducer } from 'react';
import {
  getMoviesRequest,
  Movie,
  MovieApiResponse,
  sendRequest,
} from '../api/sendRequest';
import { initialMovieState, reducer } from './reducer';

export interface UseMovies {
  movies: Movie[];
  searchQuery: string;
  setSearchQuery: (query: string) => void;
  page: number;
  numberOfPages: number;
  incrementPageNumber: () => void;
  decrementPageNumber: () => void;
}

const useMovies = (): UseMovies => {
  const [moviesState, dispatch] = useReducer(reducer, initialMovieState);

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
      getMoviesRequest(searchQuery, page)
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
    } else {
      dispatch({ ...initialMovieState });
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
