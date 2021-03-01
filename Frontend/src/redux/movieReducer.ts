import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { getMovieDetail, MovieDetail } from '../services/tmdbApi';
import {
  LoadingState,
  loadingFailed,
  loadingStarted,
  loadingSucceeded,
} from '../services/utils';

type MovieState = LoadingState & MovieDetail;

export const NAME = 'movie';

export const fetchMovieDetails = createAsyncThunk(
  `${NAME}/fetch`,
  getMovieDetail
);

const initialState = {
  id: 0,
  title: '',
  original_title: '',
  release_date: '',
  overview: '',
  poster_path: '',
  isLoading: false,
  error: null,
  timestamp: null,
} as MovieState;

const movieSlice = createSlice({
  name: NAME,
  initialState,
  reducers: {},

  extraReducers: (builder) => {
    builder.addCase(fetchMovieDetails.pending, (state) => {
      loadingStarted(state);
    });

    builder.addCase(fetchMovieDetails.fulfilled, (state, { payload }) => {
      loadingSucceeded(state);
      state.id = payload.id;
      state.title = payload.title;
      state.overview = payload.overview;
      state.original_title = payload.original_title;
      state.release_date = payload.release_date;
      state.poster_path = payload.poster_path;
    });

    builder.addCase(fetchMovieDetails.rejected, (state, action) => {
      loadingFailed(state, action);
    });
  },
});

export default movieSlice.reducer;
