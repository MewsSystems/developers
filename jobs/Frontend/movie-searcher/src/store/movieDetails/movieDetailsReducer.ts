import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { REACT_APP_TMDB_KEY, TMDB_GET_MOVIE_DETAIS_URL } from "../../constants";
import type { RootState } from "../types";
import type { RawMovieDetailsType, MovieDetailsStateType } from "./types";

const initialState: MovieDetailsStateType = {
  movie: null,
  isLoading: false,
  errorMessage: null,
};

export const getMovieDetails = createAsyncThunk("getMoviesDetails", async (movieId: string) => {
  const response = await fetch(
    `${TMDB_GET_MOVIE_DETAIS_URL}/${movieId}?${new URLSearchParams({
      api_key: REACT_APP_TMDB_KEY,
    })}`
  );
  const data = await response.json();
  return data;
});

export const movieDetailsSlice = createSlice({
  name: "movieDetails",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      //getMovieDetails
      .addCase(getMovieDetails.pending, (state) => {
        state.isLoading = true;
        state.errorMessage = null;
      })
      .addCase(getMovieDetails.fulfilled, (state, action: PayloadAction<RawMovieDetailsType>) => {
        state.isLoading = false;
        state.movie = action.payload;
      })
      .addCase(getMovieDetails.rejected, (state, action) => {
        state.isLoading = false;
        state.errorMessage = action.error.message;
      });
  },
});

export const selectMovieDetailsState = (state: RootState) => state.movieDetails;
export default movieDetailsSlice.reducer;
