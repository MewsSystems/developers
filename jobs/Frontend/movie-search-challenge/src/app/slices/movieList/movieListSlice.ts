import type { PayloadAction } from "@reduxjs/toolkit"
import type { SimpleMovie } from "./interfaces/simple-movie"
import { createSlice } from "@reduxjs/toolkit"

export interface MovieListState {
  page: number
  movies: SimpleMovie[]
  isLoading: boolean
}

const initialState: MovieListState = {
  page: 0,
  movies: [],
  isLoading: false,
}

export const movieListSlice = createSlice({
  name: "movieList",
  initialState,
  reducers: {
    startLoadingMovies: state => {
      state.isLoading = true
    },
    setMovies: (
      state,
      action: PayloadAction<{ page: number; movies: SimpleMovie[] }>,
    ) => {
      state.isLoading = false
      state.page = action.payload.page
      state.movies = action.payload.movies
    },
  },
})

export const { startLoadingMovies, setMovies } = movieListSlice.actions
