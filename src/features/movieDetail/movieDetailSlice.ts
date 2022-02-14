import {createAsyncThunk, createSlice} from '@reduxjs/toolkit';
import {RootState} from '../../app/store';
import {client} from '../../api/client';

export interface MovieGenre {
  id: number;
  name: string;
}

export interface MovieProductionCompany {
  id: number;
  logo_path: string;
  name: string;
  origin_country: string;
}

export interface MovieProductionCountries {
  iso_3166_1: string;
  name: string;
}

export interface MovieSpokenLanguages {
  english_name: string;
  iso_639_1: string;
  name: string;
}

export interface MovieDetail {
  adult: boolean;
  backdrop_path: string | null;
  belongs_to_collection: {
    id: number;
    name: string;
    poster_path: string;
    backdrop_path: string | null;
  };
  budget: number;
  genres: MovieGenre[];
  homepage: string;
  id: number;
  imdb_id: string;
  original_language: string;
  original_title: string;
  overview: string;
  popularity: number;
  poster_path: string | null;
  production_companies: MovieProductionCompany[];
  production_countries: MovieProductionCountries[];
  release_date: string;
  revenue: number;
  runtime: number;
  spoken_languages: MovieSpokenLanguages[];
  status: string;
  tagline: string;
  title: string;
  video: boolean;
  vote_average: number;
  vote_count: number;
}

export interface MovieDetailState {
  movieDetail: MovieDetail | null;
  status: 'idle' | 'loading' | 'succeeded' | 'failed';
  error: any;
}

const initialState: MovieDetailState = {
  movieDetail: null,
  status: 'idle',
  error: null,
};

export const fetchMovieDetail = createAsyncThunk('movie', async (movieId: number) => {
  const response = await client.get(`movie/${movieId}`, {
    api_key: process.env.REACT_APP_API_KEY,
  });
  return response.data;
});

export const movieDetailSlice = createSlice({
  name: 'movieDetail',
  initialState,
  reducers: {
    clearMovieDetail: (state) => {
      state.movieDetail = null;
      state.status = 'idle';
      state.error = null;
    },
  },
  extraReducers(builder) {
    builder
      .addCase(fetchMovieDetail.pending, (state) => {
        state.status = 'loading';
      })
      .addCase(fetchMovieDetail.fulfilled, (state, action) => {
        state.status = 'succeeded';
        state.movieDetail = action.payload;
      })
      .addCase(fetchMovieDetail.rejected, (state, action) => {
        state.status = 'failed';
        state.error = action.error.message;
      });
  },
});
export const { clearMovieDetail } = movieDetailSlice.actions;

export const getMovieDetail = (state: RootState) => state.movieDetail.movieDetail;

export default movieDetailSlice.reducer;
