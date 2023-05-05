import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { TMDB_SEARCH_MOVIES_URL } from "../../constants";
import type { RootState } from "../types";
import type { MovieSearchStateType, MoviesFoundType } from "./types";

const initialState: MovieSearchStateType = {
  moviesFound: {
    page: null,
    results: [],
    total_pages: null,
    total_results: null,
  },
  isLoading: false,
  errorMessage: null,
};

export const getMoviesList = createAsyncThunk("moviesList", async (search: string) => {
  const response = await fetch(
    `${TMDB_SEARCH_MOVIES_URL}?${new URLSearchParams({
      api_key: "03b8572954325680265531140190fd2a",
      query: search,
    })}`
  );

  console.log("response", response);

  const data = await response.json();
  return data;
});

export const movieSearchSlice = createSlice({
  name: "movieSearch",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(getMoviesList.pending, (state) => {
        state.isLoading = true;
        state.errorMessage = null;
      })
      .addCase(getMoviesList.fulfilled, (state, action: PayloadAction<MoviesFoundType>) => {
        state.isLoading = false;

        console.log("action.payload", action.payload);

        state.moviesFound = action.payload;
      })
      .addCase(getMoviesList.rejected, (state, action) => {
        state.isLoading = false;
        state.errorMessage = action.error.message;
      });
  },
});

export const selectMoviesListState = (state: RootState) => state.moviesList;
export default movieSearchSlice.reducer;
