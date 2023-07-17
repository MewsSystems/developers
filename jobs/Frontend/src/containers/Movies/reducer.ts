import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { getMovies } from "../../api";
import { AppDispatch } from "../../store";
import { GetMoviesResponse, Movie } from "./types";

type State = {
  movies: Movie[];
  currentPage: number;
  totalPages: number;
  fetchState: "idle" | "loading" | "success" | "error" | "loading-more";
  searchQuery: string;
};

const initialState: State = {
  movies: [],
  currentPage: 1,
  totalPages: 1,
  fetchState: "idle",
  searchQuery: "",
};

export const moviesReducer = createSlice({
  name: "movies",
  initialState,
  reducers: {
    loadingMoviesSuccess: (state, action: PayloadAction<GetMoviesResponse>) => {
      const { results, page, total_pages } = action.payload;

      state.currentPage = page;
      state.totalPages = total_pages;
      state.movies = page === 1 ? results : state.movies.concat(results);
      state.fetchState = "success";
    },
    loadingMovies: (state) => {
      state.fetchState = "loading";
    },
    loadingMoreMovies: (state) => {
      state.fetchState = "loading-more";
    },
    loadingMoviesError: (state) => {
      state.fetchState = "error";
    },
    changeSearchQuery: (state, action: PayloadAction<string>) => {
      state.searchQuery = action.payload;
    },
  },
});

export const {
  loadingMoviesSuccess,
  loadingMovies,
  loadingMoreMovies,
  loadingMoviesError,
  changeSearchQuery,
} = moviesReducer.actions;

export const searchMovie =
  (searchQuery: string, currentPage = 1) =>
  (dispatch: AppDispatch) => {
    if (!searchQuery) {
      dispatch(changeSearchQuery(""));
      return;
    }

    if (currentPage > 1) {
      dispatch(loadingMoreMovies());
    } else {
      dispatch(changeSearchQuery(searchQuery));
      dispatch(loadingMovies());
    }

    getMovies(searchQuery, currentPage)
      .then((movies) => {
        // just adding a slight delay so that the loading states can be properly seen.
        setTimeout(() => {
          dispatch(loadingMoviesSuccess(movies));
        }, 500);
      })
      .catch(() => dispatch(loadingMoviesError()));
  };
