import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { TMDB_SEARCH_MOVIES_URL } from "../../constants";
import type { RootState } from "../types";
import { cacheData, getCachedData } from "./cacheData.util";
import type { MovieSearchStateType, MoviesFoundType } from "./types";

const initialState: MovieSearchStateType = {
  moviesList: [],
  totalPages: null,
  totalResults: null,
  currentPage: 1,
  inputValue: undefined,
  isLoading: false,
  errorMessage: null,
  visiblePages: [],
};

const fetchMoviesListFactory = (name: string) =>
  createAsyncThunk(name, async ({ value, page = 1 }: { value: string; page?: number }) => {
    const cachedData = getCachedData(value, page);
    if (cachedData) {
      return cachedData;
    }

    const response = await fetch(
      `${TMDB_SEARCH_MOVIES_URL}?${new URLSearchParams({
        api_key: "03b8572954325680265531140190fd2a",
        query: value,
        page: page.toString(),
        adult: "false",
      })}`
    );
    const data = await response.json();
    cacheData(value, page, data);
    return data;
  });

export const getMoviesList = fetchMoviesListFactory("getMoviesList");
export const loadMoreMoviesList = fetchMoviesListFactory("loadMoreMovies");

export const moviesFoundSlice = createSlice({
  name: "moviesFound",
  initialState,
  reducers: {
    setInputValue: (state, action: PayloadAction<string>) => {
      state.inputValue = action.payload;
    },
    setCurrentPage: (state, action: PayloadAction<number>) => {
      state.currentPage = action.payload;
      state.visiblePages = [];
    },
  },
  extraReducers: (builder) => {
    builder
      //getMoviesList
      .addCase(getMoviesList.pending, (state) => {
        state.isLoading = true;
        state.errorMessage = null;
      })
      .addCase(getMoviesList.fulfilled, (state, action: PayloadAction<MoviesFoundType>) => {
        state.isLoading = false;
        state.moviesList = action.payload.results;
        state.totalPages = action.payload.total_pages;
        state.totalResults = action.payload.total_results;
      })
      .addCase(getMoviesList.rejected, (state, action) => {
        state.isLoading = false;
        state.errorMessage = action.error.message;
      })

      //loadMoreMoviesList
      .addCase(loadMoreMoviesList.pending, (state) => {
        state.isLoading = true;
        state.errorMessage = null;
      })
      .addCase(loadMoreMoviesList.fulfilled, (state, action: PayloadAction<MoviesFoundType>) => {
        state.isLoading = false;
        state.moviesList = [...state.moviesList, ...action.payload.results];
        state.currentPage = state.currentPage + 1;
        state.visiblePages.push(state.currentPage, state.currentPage + 1);
      })
      .addCase(loadMoreMoviesList.rejected, (state, action) => {
        state.isLoading = false;
        state.errorMessage = action.error.message;
      });
  },
});

export const { setCurrentPage, setInputValue } = moviesFoundSlice.actions;
export const selectMoviesListState = (state: RootState) => state.moviesFound;
export default moviesFoundSlice.reducer;
