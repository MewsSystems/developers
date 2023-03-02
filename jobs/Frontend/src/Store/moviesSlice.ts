import { createSlice, createAsyncThunk } from '@reduxjs/toolkit'
import type { PayloadAction } from '@reduxjs/toolkit';
import tmdbApi, { Movie } from '../API';
import { SearchMovieResult, MovieListResult } from '../API';
import { RootState } from './store';

interface MovieState {
  movieListResult: MovieListResult[];
  movieDetails: Movie | null;
  searchTerm: string;
  page: number;
  totalPages: number;
  totalResults: number;
  status: 'idle' | 'pending' | 'fulfilled' | 'rejected';
}

const initialState: MovieState = {
  movieListResult: [],
  movieDetails: null,
  searchTerm: '',
  page: 1,
  totalPages: 1,
  totalResults: 0,
  status: 'idle',
}

export const fetchMovies = createAsyncThunk(
  'movies/fetchMovies', 
  async ({ searchTerm, page }: { searchTerm: string, page: string }, { dispatch }) => {
    dispatch(searchTermUpdated({ searchTerm: searchTerm }));
    const movies = await tmdbApi.searchMovie(searchTerm, page);
    return movies;
  }
);

export const fetchMovieDetails = createAsyncThunk(
  'movies/fetchMovieDetails',
  async({ movieId }: { movieId: string }, { dispatch }) => {
    return await tmdbApi.movieDetails(movieId);
  }
)

const moviesSlice = createSlice({
  name: 'movies',
  initialState,
  reducers: {
    searchTermUpdated(state, action) {
      return { ...state, ...action.payload }
    },
  },
  extraReducers: (builder) => {
    builder.addCase(fetchMovies.pending, (state, action) => {
      return { ...state, status: 'pending' }
    });
    builder.addCase(fetchMovies.fulfilled, (state, action: PayloadAction<SearchMovieResult>) => {
      if (action.payload.page <= action.payload.total_pages) {
        return { 
          ...state, 
          movieListResult: action.payload.results,
          page: action.payload.page,
          totalPages: action.payload.total_pages,
          totalResults: action.payload.total_results, 
          status: 'fulfilled',
        };
      }
      else {
        return { ...state, status: 'rejected' }
      }
    });
    builder.addCase(fetchMovies.rejected, (state, action) => {
      return { ...state, status: 'rejected' }
    })
    builder.addCase(fetchMovieDetails.pending, (state, action) => {
      return { ...state, status: 'pending' }
    });
    builder.addCase(fetchMovieDetails.fulfilled, (state, action: PayloadAction<Movie>) => {
      return { ...state, movieDetails: action.payload, status: 'fulfilled' }
    });
    builder.addCase(fetchMovieDetails.rejected, (state, action) => {
      return { ...state, status: 'rejected' }
    })
  },
})

export const { searchTermUpdated } = moviesSlice.actions;

export default moviesSlice.reducer;

export const selectMovieResults = (state: RootState) => state.movies.movieListResult;
export const selectSearchTerm = (state: RootState) => state.movies.searchTerm;
export const selectMovieDetails = (state: RootState) => state.movies.movieDetails;
export const selectNavigationDetails = (state: RootState) => { 
  return {
    page: state.movies.page,
    totalPages: state.movies.totalPages,
    totalResults: state.movies.totalResults,
  }
};
export const selectLoadingStatus = (state: RootState) => state.movies.status;
