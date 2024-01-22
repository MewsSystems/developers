import type { PayloadAction } from "@reduxjs/toolkit"
import type { SimpleMovie } from "./interfaces/simple-movie"
import { createSlice } from "@reduxjs/toolkit"

export interface MovieListState {
  page: number
  movies: SimpleMovie[]
  isLoading: boolean
  moreResults: boolean
  searchQuery: string
}

const initialState: MovieListState = {
  page: 1,
  movies: [],
  isLoading: false,
  moreResults: false,
  searchQuery: "",
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
    setSearchQuery: (state, action: PayloadAction<{ searchQuery: string }>) => {
      state.searchQuery = action.payload.searchQuery
    },
  },
})

export const { startLoadingMovies, setMovies, setSearchQuery } =
  movieListSlice.actions
