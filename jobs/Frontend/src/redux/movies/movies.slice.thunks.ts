import { createAsyncThunk } from "@reduxjs/toolkit";
import { MoviesAPI } from "../../api/movies";
import { MovieSearch } from "./movies.slice.types";
import { RootState } from "../store";

const searchMovies = createAsyncThunk<
  MovieSearch | undefined,
  string,
  { state: RootState; rejectWithValue: Error }
>("movies/searchMovies", async (query: string, thunkApi) => {
  const currentPage = thunkApi.getState().movies.search.page;
  try {
    return MoviesAPI.search(query, currentPage);
  } catch (error) {
    thunkApi.rejectWithValue(error);
  }
});

export const moviesThunks = {
  searchMovies,
};
