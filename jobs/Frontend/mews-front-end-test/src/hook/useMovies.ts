import { useCallback, useEffect, useReducer } from 'react';
import {
  getMoviesRequest,
  Movie,
  MovieApiResponse,
  sendRequest,
} from '../api/sendRequest';
import { initialMovieState, reducer } from './reducer';
import { useDispatch } from 'react-redux';
import { Dispatch } from '@reduxjs/toolkit';

export interface UseMovies {
  movies: Movie[];
  searchQuery: string;
  setSearchQuery: (query: string) => void;
  page: number;
  numberOfPages: number;
  incrementPageNumber: () => void;
  decrementPageNumber: () => void;
  dispatch: Dispatch;
}

const useMovies = (): UseMovies => {
  const reduxDispatch = useDispatch();

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
    dispatch({ searchQuery: query, page: 1 });
  };

  const sendMovieRequest = useCallback(sendRequest, [sendRequest]);

  useEffect(() => {
    if (Boolean(searchQuery)) {
      getMoviesRequest(searchQuery, page)
        .then((response: MovieApiResponse) => {
          dispatch({
            movies: response.results,
            numberOfPages: response.total_pages,
            page: response.page,
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
    numberOfPages,
    page,
    setSearchQuery,
    incrementPageNumber,
    decrementPageNumber,
    dispatch: reduxDispatch,
  };
};

export { useMovies };
