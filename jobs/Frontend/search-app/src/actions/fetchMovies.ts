import { createAsyncThunk } from "@reduxjs/toolkit";
import { AppDispatch, RootState } from "../app/store";
import {
  updateActivePage,
  updateMovieDetailId,
  updateSearchKey,
} from "../reducers/moviesListSlice";
import { MovieDetailResult, MoviesSearchResponse } from "../app/types";

const API_KEY = "03b8572954325680265531140190fd2a";
const MOVIE_API_URL = "https://api.themoviedb.org/3/";

export const fetchMovies =
  (key: string, page: number) =>
  (dispatch: AppDispatch, getState: () => RootState): void => {
    const state = getState();
    const { searchKey, activePage } = state.moviesList;

    // doesn't perform any changes in case the search key and active page haven't changed
    if (searchKey === key && activePage === page) {
      return;
    }

    dispatch(updateSearchKey(key));
    dispatch(updateActivePage(page));
    dispatch(fetchMoviesThunk());
  };

export const fetchMovieDetail =
  (movieId: string) =>
  (dispatch: AppDispatch, getState: () => RootState): void => {
    const state = getState() as RootState;
    const { movieDetailId } = state.moviesList;

    // doesn't perform any changes in case the movieId hasn't changed
    if (movieId === movieDetailId) {
      return;
    }

    dispatch(updateMovieDetailId(movieId));
    dispatch(fetchMovieDetailThunk());
  };

export const fetchMoviesThunk = createAsyncThunk(
  "moviesList/fetchMovies",
  async (arg, { getState }): Promise<MoviesSearchResponse> => {
    const state = getState() as RootState;
    const { searchKey, activePage } = state.moviesList;
    const response = await fetch(
      `${MOVIE_API_URL}search/movie?api_key=${API_KEY}&query=${searchKey}&page=${activePage}`
    );
    return await response.json();
  }
);

export const fetchMovieDetailThunk = createAsyncThunk(
  "moviesList/fetchMovie",
  async (arg, { getState }): Promise<MovieDetailResult> => {
    const state = getState() as RootState;
    const { movieDetailId } = state.moviesList;

    const response = await fetch(
      `${MOVIE_API_URL}movie/${movieDetailId}?api_key=${API_KEY}`
    );
    return await response.json();
  }
);
