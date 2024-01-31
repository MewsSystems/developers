import type { PayloadAction } from "@reduxjs/toolkit"
import type { SimpleMovieDetails } from "./interfaces/simple-movie-details"
import { createSlice } from "@reduxjs/toolkit"

export interface MovieDetailsState {
  details: SimpleMovieDetails
  isLoading: boolean
}

export const initialState: MovieDetailsState = {
  details: {
    image: "",
    title: "",
    tagline: "",
    language: "",
    length: 0,
    rate: 0,
    budget: 0,
    release_date: "",
    genres: [
      {
        id: 0,
        name: "",
      },
    ],
    overview: "",
  },
  isLoading: false,
}

export const movieDetailsSlice = createSlice({
  name: "movieDetails",
  initialState,
  reducers: {
    startLoadingDetails: state => {
      state.isLoading = true
    },
    setDetails: (
      state,
      action: PayloadAction<{
        movieDetails: SimpleMovieDetails
      }>,
    ) => {
      state.details = action.payload.movieDetails
      state.isLoading = false
    },
  },
})

export const { startLoadingDetails, setDetails } = movieDetailsSlice.actions
