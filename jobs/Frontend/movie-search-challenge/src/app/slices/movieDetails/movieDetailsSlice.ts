import type { PayloadAction } from "@reduxjs/toolkit"
import type { SimpleMovieDetails } from "./interfaces/simple-movie-details"
import { createSlice } from "@reduxjs/toolkit"

export interface MovieDetailsState {
  details: SimpleMovieDetails
  isLoading: boolean
}

const initialState: MovieDetailsState = {
  details: {} as SimpleMovieDetails,
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
