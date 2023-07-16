import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BaseQueryFn } from "src/baseQuery";
import { GetMoviesParams } from "src/store/slices/interfaces/GetMoviesParams";
import type { MovieType } from "src/store/types/MovieType";

const baseQuery: BaseQueryFn = fetchBaseQuery({
  baseUrl: "https://api.themoviedb.org/3",
});

export const moviesApi = createApi({
  reducerPath: "moviesApi",
  baseQuery,
  endpoints: (builder) => ({
    getMovies: builder.query<MovieType, GetMoviesParams>({
      query: ({ name, page = 1 }) =>
        `/search/movie?query=${name}&language=en-US&page=${page}&api_key=${
          import.meta.env.VITE_MOVIES_API_KEY
        }`,
    }),
    getMovieById: builder.query<MovieType, string>({
      query: (movieId) =>
        `movie/${movieId}?api_key=${import.meta.env.VITE_MOVIES_API_KEY}`,
    }),
  }),
});

export const {
  useGetMoviesQuery,
  useLazyGetMoviesQuery,
  useGetMovieByIdQuery,
  useLazyGetMovieByIdQuery,
} = moviesApi;
