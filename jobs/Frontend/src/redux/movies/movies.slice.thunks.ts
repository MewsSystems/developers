import { createAsyncThunk } from "@reduxjs/toolkit";
import { MoviesAPI } from "../../api/movies";
import { MovieSearch } from "./movies.slice.types";
import { RootState } from "../store";
import { changeLoading, setSelectedMovie } from "./movies.slice";

const searchMovies = createAsyncThunk<
  MovieSearch | undefined,
  string,
  { state: RootState; rejectWithValue: Error }
>("movies/searchMovies", async (query: string, thunkApi) => {
  const { getState, dispatch, rejectWithValue } = thunkApi;
  dispatch(changeLoading(true));

  const currentPage = getState().movies.search.page;
  try {
    return MoviesAPI.search(query, currentPage);
  } catch (error) {
    rejectWithValue(error);
  } finally {
    dispatch(changeLoading(false));
  }
});

const fetchMovieDetails = createAsyncThunk<
  void,
  number,
  { state: RootState; rejectWithValue: Error }
>("movies/fetchMovieDetails", async (id: number, thunkApi) => {
  const { dispatch, rejectWithValue } = thunkApi;
  dispatch(changeLoading(true));
  try {
    const movie = await MoviesAPI.getDetails(id);
    dispatch(setSelectedMovie(movie));
  } catch (error) {
    rejectWithValue(error);
  } finally {
    dispatch(changeLoading(false));
  }
});

export const moviesThunks = {
  searchMovies,
  fetchMovieDetails,
};
