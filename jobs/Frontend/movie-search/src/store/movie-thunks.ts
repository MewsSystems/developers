import { createAsyncThunk } from "@reduxjs/toolkit";
import { MovieDetail, MoviesPage } from "../models/movies.types";
import { getMovieDetailEndpoint, searchMoviesEndpoint } from "../services/movies-service";




export const searchMoviesThunk = createAsyncThunk<
  MoviesPage,
  { query: string; page: number }
>("movies/search", async ({ query, page }) => {
  const response: MoviesPage = await searchMoviesEndpoint(query, page);
  return response;
});

export const getMovieDetailThunk = createAsyncThunk<
MovieDetail,
  { movieId:number }
>("movies/detail", async ({ movieId }) => {
  const response: MovieDetail = await getMovieDetailEndpoint(movieId);
  return response;
});
