import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import {RootState} from '../../app/store';
import {client} from '../../api/client';

export interface MovieItem {
  id: number;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  release_date: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
  adult: boolean;
  backdrop_path: string | null;
  genre_ids: number[];
}

export interface MoviesState {
  movies: MovieItem[];
  total_pages: number;
  total_results: number;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: any;
}

export interface MovieSearchParams {
  query: string;
  page: number;
}

const initialState: MoviesState = {
  movies: [],
  status: 'idle',
  error: null,
  total_pages: 0,
  total_results: 0,
};

export const searchMovies = createAsyncThunk('search/movie', async (params: MovieSearchParams) => {
  const response = await client.get('search/movie', {
    query: params.query,
    page: params.page,
    api_key: process.env.REACT_APP_API_KEY,
  });
  return response.data;
});

export const moviesSlice = createSlice({
  name: 'movies',
  initialState,
  reducers: {
    clearMovies: (state) => {
      state.movies = [];
      state.total_pages = 0;
      state.total_results = 0;
      state.status = 'idle';
      state.error = null;
    },
  },
  extraReducers(builder) {
    builder
      .addCase(searchMovies.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(searchMovies.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.movies = action.payload.results;
        state.total_pages = action.payload.total_pages;
        state.total_results = action.payload.total_results;
      })
      .addCase(searchMovies.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      });
  },
});

export const {clearMovies} = moviesSlice.actions;

export const selectAllSearchedMovies = (state: RootState) => state.movies.movies;

export const selectTotalPages = (state: RootState) => state.movies.total_pages;

export default moviesSlice.reducer;
