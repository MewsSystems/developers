import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { getMovieDetail, MovieDetail, MovieParams } from '../services/tmdbApi';
import {
  RequestState,
  loadingFailed,
  loadingStarted,
  loadingSucceeded,
} from '../services/utils';
import { AppSelector, RootState } from '../store';
import { updateState } from './utils';

type MovieState = RequestState & MovieDetail;

export const NAME = 'movie';

export const fetchMovieDetails = createAsyncThunk<
  MovieDetail,
  MovieParams,
  {
    state: RootState;
  }
>(`${NAME}/fetch`, getMovieDetail, {
  condition: ({ movieId }, { getState }) => {
    const movie = movieSelector(getState());
    return parseInt(movieId, 10) !== movie.id;
  },
});

const initialState = {
  id: 0,
  title: '',
  original_title: '',
  original_language: '',
  release_date: '',
  overview: '',
  poster_path: '',
  vote_average: 0,
  vote_count: 0,
  genres: [],
  isLoading: false,
  error: null,
  timestamp: null,
  runtime: 0,
} as MovieState;

const movieSlice = createSlice({
  name: NAME,
  initialState,
  reducers: {},

  extraReducers: (builder) => {
    builder.addCase(fetchMovieDetails.pending, (state) => {
      updateState(state, initialState);
      loadingStarted(state);
    });

    builder.addCase(fetchMovieDetails.fulfilled, (state, { payload }) => {
      loadingSucceeded(state);
      updateState(state, payload);
    });

    builder.addCase(fetchMovieDetails.rejected, (state, action) => {
      loadingFailed(state, action);
    });
  },
});

export const movieSelector: AppSelector<MovieState> = (state) =>
  state[movieSlice.name];

export default movieSlice.reducer;
