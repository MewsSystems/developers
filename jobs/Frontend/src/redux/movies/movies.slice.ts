import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { moviesThunks } from "./movies.slice.thunks";
import { Movie, MoviesState } from "./movies.slice.types";

// Define the initial state using that type
const initialState: MoviesState = {
  search: {
    results: [],
    page: 1,
    total_pages: null,
    loading: false,
  },
  query: "",
  selectedMovie: null,
};

export const moviesSlice = createSlice({
  name: "movies",
  // `createSlice` will infer the state type from the `initialState` argument
  initialState,
  reducers: {
    setSelectedMovie: (state, action: PayloadAction<Movie>) => {
      state.selectedMovie = action.payload;
    },
    setQuery: (state, action: PayloadAction<string>) => {
      state.query = action.payload;
    },
    changeLoading: (state, action: PayloadAction<boolean>) => {
      state.search.loading = action.payload;
    },
    clearSelectedMovie: (state, action: PayloadAction<number>) => {
      if (state.selectedMovie?.id !== action.payload) {
        state.selectedMovie = null;
      }
    },
  },
  extraReducers: (builder) => {
    builder.addCase(moviesThunks.searchMovies.fulfilled, (state, action) => {
      if (action.payload) {
        state.search.results = action.payload.results;
        state.search.total_pages = action.payload.total_pages;
        if (
          action.payload.total_pages &&
          action.payload.total_pages > state.search.page
        ) {
          state.search.page += 1;
        }
      }
    });
  },
});

export const { setSelectedMovie, setQuery, changeLoading, clearSelectedMovie } =
  moviesSlice.actions;

export default moviesSlice.reducer;
