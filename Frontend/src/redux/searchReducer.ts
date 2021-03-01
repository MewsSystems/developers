import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { SearchMovieResults, getMovieSearchResults } from '../services/tmdbApi';
import {
  LoadingState,
  loadingStarted,
  loadingSucceeded,
  loadingFailed,
} from '../services/utils';
import { isActionAborted } from './utils';

type SearchState = LoadingState &
  SearchMovieResults & {
    query: string;
  };

export const NAME = 'search';

export const fetchSearchResults = createAsyncThunk(
  `${NAME}/fetchResults`,
  getMovieSearchResults
);

const initialState = {
  query: '',
  results: [],
  page: 1,
  total_pages: 1,
  total_results: 0,
  isLoading: false,
  error: null,
  timestamp: null,
} as SearchState;

const searchSlice = createSlice({
  name: NAME,
  initialState,
  reducers: {
    clear() {
      return initialState;
    },
  },

  extraReducers: (builder) => {
    builder.addCase(fetchSearchResults.pending, (state, { meta }) => {
      loadingStarted(state);

      // clear cached results
      if (state.query !== meta.arg.query) {
        state.results = initialState.results;
      }

      state.query = meta.arg.query;
      state.page = meta.arg.page || initialState.page;
    });

    builder.addCase(fetchSearchResults.fulfilled, (state, { payload }) => {
      loadingSucceeded(state);
      state.page = payload.page;
      state.total_results = payload.total_results;
      state.total_pages = payload.total_pages;
      state.results = payload.results;
    });

    builder.addCase(fetchSearchResults.rejected, (state, action) => {
      if (!isActionAborted(action)) {
        loadingFailed(state, action);
      }
    });
  },
});

export const { clear } = searchSlice.actions;

export default searchSlice.reducer;
