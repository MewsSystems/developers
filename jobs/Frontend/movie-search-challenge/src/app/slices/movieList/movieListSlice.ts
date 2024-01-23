import type { PayloadAction } from "@reduxjs/toolkit"
import type { SimpleMovie } from "./interfaces/simple-movie"
import { createSlice } from "@reduxjs/toolkit"

export interface MovieListState {
  page: number
  movies: SimpleMovie[]
  isLoading: boolean
  isLoadingMoreResults: boolean
  morePages: boolean
  searchQuery: string
}

const initialState: MovieListState = {
  page: 1,
  movies: [],
  isLoading: false,
  isLoadingMoreResults: false,
  morePages: false,
  searchQuery: "",
}

export const movieListSlice = createSlice({
  name: "movieList",
  initialState,
  reducers: {
    startLoadingMovies: state => {
      state.isLoading = true
    },
    startAddingResults: state => {
      state.isLoadingMoreResults = true
    },
    setMovies: (
      state,
      action: PayloadAction<{
        page: number
        movies: SimpleMovie[]
        totalPages: number
      }>,
    ) => {
      state.isLoading = false
      state.isLoadingMoreResults = false
      state.page = action.payload.page
      state.movies = action.payload.movies
      state.morePages = action.payload.page < action.payload.totalPages
    },
    setSearchQuery: (state, action: PayloadAction<{ searchQuery: string }>) => {
      state.searchQuery = action.payload.searchQuery
    },
  },
})

export const {
  startLoadingMovies,
  startAddingResults,
  setMovies,
  setSearchQuery,
} = movieListSlice.actions
