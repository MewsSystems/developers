import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { Movie } from '../../api/sendRequest';

export interface MovieState {
  movies: Movie[];
  currentMovie: Movie;
  searchQuery: string;
  page: number;
  numberOfPages: number;
}

const initialMovieState: MovieState = {
  movies: [],
  currentMovie: {} as Movie,
  searchQuery: '',
  page: 1,
  numberOfPages: 1,
};

const movieSlice = createSlice({
  name: 'movies',
  initialState: initialMovieState,
  reducers: {
    setCurrentMovie: (state, action: PayloadAction<Movie>) => {
      state.currentMovie = action.payload;
    },
    setCurrentSearch: (
      state,
      action: PayloadAction<Omit<MovieState, 'currentMovie'>>,
    ) => {
      state.movies = action.payload.movies;
      state.searchQuery = action.payload.searchQuery;
      state.page = action.payload.page;
      state.numberOfPages = action.payload.numberOfPages;
    },
  },
});

export const { setCurrentSearch, setCurrentMovie } = movieSlice.actions;

export default movieSlice.reducer;
