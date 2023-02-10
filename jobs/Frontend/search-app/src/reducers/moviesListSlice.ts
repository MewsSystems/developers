import { createSlice, PayloadAction } from "@reduxjs/toolkit";

import { MoviesListState } from "../app/types";
import {
  fetchMovieDetailThunk,
  fetchMoviesThunk,
} from "../actions/fetchMovies";

const initialState: MoviesListState = {
  searchKey: "",
  movies: [],
  isBusy: false,
  error: null,
  activePage: 0,
  totalPages: 0,
  results: 0,
  movieDetail: null,
  movieDetailId: "",
};

export const moviesListSlice = createSlice({
  name: "moviesList",
  initialState,
  reducers: {
    updateSearchKey: (
      state: MoviesListState,
      action: PayloadAction<string>
    ) => {
      state.searchKey = action.payload;
    },
    updateActivePage: (
      state: MoviesListState,
      action: PayloadAction<number>
    ) => {
      state.activePage = action.payload;
    },
    clearMovies: (state: MoviesListState) => {
      state.movies = [];
      state.results = 0;
      state.searchKey = "";
      state.activePage = 0;
    },
    updateMovieDetailId: (
      state: MoviesListState,
      action: PayloadAction<string>
    ) => {
      state.movieDetailId = action.payload;
      state.movieDetail = null;
    },
  },
  extraReducers: builder => {
    builder
      .addCase(fetchMoviesThunk.pending, state => {
        state.isBusy = true;
      })
      .addCase(fetchMoviesThunk.fulfilled, (state, action) => {
        state.isBusy = false;
        state.movies = action.payload.results;
        state.activePage = action.payload.page;
        state.totalPages = action.payload.total_pages;
        state.results = action.payload.total_results;
      })
      .addCase(fetchMoviesThunk.rejected, (state, action) => {
        state.isBusy = false;
        state.error = action.error;
      })
      .addCase(fetchMovieDetailThunk.pending, state => {
        state.isBusy = true;
      })
      .addCase(fetchMovieDetailThunk.fulfilled, (state, action) => {
        state.isBusy = false;
        state.movieDetail = action.payload;
      })
      .addCase(fetchMovieDetailThunk.rejected, (state, action) => {
        state.isBusy = false;
        state.error = action.error;
      });
  },
});

export const {
  updateSearchKey,
  updateActivePage,
  clearMovies,
  updateMovieDetailId,
} = moviesListSlice.actions;

export const moviesListReducer = moviesListSlice.reducer;
