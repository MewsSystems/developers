import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import {
  SearchMovieResults,
  getMovieSearchResults,
  SearchMovieParams,
} from '../services/tmdbApi';
import {
  RequestState,
  loadingStarted,
  loadingSucceeded,
  loadingFailed,
} from '../services/utils';
import { AppSelector, RootState } from '../store';
import { isActionAborted, updateState } from './utils';

type SearchState = RequestState &
  SearchMovieResults & {
    query: string;
  };

export const NAME = 'search';

export const fetchSearchResults = createAsyncThunk<
  SearchMovieResults,
  SearchMovieParams,
  {
    state: RootState;
  }
>(`${NAME}/fetchResults`, getMovieSearchResults, {
  condition: ({ query, page = initialState.page }, { getState }) => {
    const { search } = getState();
    const isEmptyQuery = query === '';
    const isCached = query === search.query && page === search.page;
    return !isEmptyQuery && !isCached;
  },
});

export const setSearchPage = createAsyncThunk<
  SearchMovieResults,
  SearchMovieParams['page'],
  {
    state: RootState;
    dispatch: any;
  }
>(
  `${NAME}/setPage`,
  async (page, { getState, dispatch }) => {
    const query = querySelector(getState());
    const action = await dispatch(fetchSearchResults({ query, page }));
    return action;
  },
  {
    condition: (_, { getState }) => {
      const query = querySelector(getState());
      return query !== initialState.query;
    },
  }
);

export const initialState = {
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
      updateState(state, payload);
    });

    builder.addCase(fetchSearchResults.rejected, (state, action) => {
      if (!isActionAborted(action)) {
        loadingFailed(state, action);
      }
    });
  },
});

export const { clear } = searchSlice.actions;

export const searchSelector: AppSelector<SearchState> = (state) =>
  state[searchSlice.name];
export const querySelector: AppSelector<SearchState['query']> = (state) =>
  searchSelector(state).query;
export const pageSelector: AppSelector<SearchState['page']> = (state) =>
  searchSelector(state).page;

export default searchSlice.reducer;
