// eslint-disable-next-line import/named
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { MovieDetail, MoviesPage } from "../models/movies.types";
import { getMovieDetailThunk, searchMoviesThunk } from "./movie-thunks";

interface MoviesState {
  foundMoviesPage?: MoviesPage;
  selectedDetail?: MovieDetail;
  statusMoviesPage: "loading" | "idle" | "init" | "empty";
  query: string;
}

const initialState: MoviesState = {
  foundMoviesPage: undefined,
  selectedDetail: undefined,
  statusMoviesPage: "init",
  query: "",
};

export const MoviesSlice = createSlice({
  name: "movies",
  initialState,
  reducers: {
    setQuery: (
      state: MoviesState,
      action: PayloadAction<{ query: string }>
    ) => {
      state.query = action.payload.query;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(searchMoviesThunk.pending, (state) => {
      state.statusMoviesPage = "loading";
    });

    builder.addCase(searchMoviesThunk.fulfilled, (state, { payload }) => {
      if (payload.total === 0) {
        state.statusMoviesPage = "empty";
        state.foundMoviesPage = payload;
      } else {
        state.statusMoviesPage = "idle";
        state.foundMoviesPage = payload;
      }
    });

    builder.addCase(getMovieDetailThunk.pending, (state) => {
      state.statusMoviesPage = "loading";
    });

    builder.addCase(getMovieDetailThunk.rejected, (state) => {
      state.statusMoviesPage = "empty";
    });

    builder.addCase(getMovieDetailThunk.fulfilled, (state, { payload }) => {
      state.statusMoviesPage = "idle";
      state.selectedDetail = payload;
    });
  },
});

export default MoviesSlice.reducer;
export const { setQuery } = MoviesSlice.actions;
