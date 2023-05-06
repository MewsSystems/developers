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
  currentPage: 1,
  isLoading: false,
  errorMessage: null,
};

export const getMoviesList = createAsyncThunk(
  "moviesFound",
  async ({ value, page }: { value: string; page: number }) => {
    const response = await fetch(
      `${TMDB_SEARCH_MOVIES_URL}?${new URLSearchParams({
        api_key: "03b8572954325680265531140190fd2a",
        query: value,
        page: page.toString(),
      })}`
    );

    console.log("response", response);

    const data = await response.json();
    return data;
  }
);

export const moviesFoundSlice = createSlice({
  name: "moviesFound",
  initialState,
  reducers: {
    setCurrentPage: (state, action: PayloadAction<number>) => {
      state.currentPage = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(getMoviesList.pending, (state) => {
        state.isLoading = true;
        state.errorMessage = null;
      })
      .addCase(getMoviesList.fulfilled, (state, action: PayloadAction<MoviesFoundType>) => {
        state.isLoading = false;
        state.moviesFound = action.payload;
      })
      .addCase(getMoviesList.rejected, (state, action) => {
        state.isLoading = false;
        state.errorMessage = action.error.message;
      });
  },
});

export const { setCurrentPage } = moviesFoundSlice.actions;

export const selectMoviesListState = (state: RootState) => state.moviesFound;
export default moviesFoundSlice.reducer;
