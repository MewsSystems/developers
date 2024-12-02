import { createAsyncThunk } from "@reduxjs/toolkit";
import { MoviesAPI } from "../../api/movies";
import { MovieSearch } from "./movies.slice.types";
import { RootState } from "../store";
import { changeLoading, resetSearch, setSelectedMovie } from "./movies.slice";
import { sleep } from "../../utils/time";

const searchMovies = createAsyncThunk<
  MovieSearch | undefined,
  { resetResults: boolean },
  { state: RootState; rejectWithValue: Error }
>("movies/searchMovies", async ({ resetResults }, thunkApi) => {
  const { getState, dispatch, rejectWithValue } = thunkApi;
  if (resetResults) {
    dispatch(resetSearch());
  }

  dispatch(changeLoading(true));

  const moviesState = getState().movies;

  const page = moviesState.search.page;
  const query = moviesState.query;
  try {
    // Simulate a delay to show the loading state
    await sleep(Math.random() * 1500);
    return MoviesAPI.search(query, page + 1);
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
