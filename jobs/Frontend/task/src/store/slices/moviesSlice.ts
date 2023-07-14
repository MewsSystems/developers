import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BaseQueryFn } from "src/baseQuery";
import type { MovieType } from "src/store/types/MovieType";

// Define a service using a base URL and expected endpoints

const baseQuery: BaseQueryFn<string, unknown> = fetchBaseQuery({
  baseUrl: "https://api.themoviedb.org/3/search/",
});

export const moviesApi = createApi({
  reducerPath: "moviesApi",
  baseQuery,
  endpoints: (builder) => ({
    getMovies: builder.query<MovieType, MovieQueryParams>({
      query: ({ name, page = 1 }) =>
        `movie?query=${name}&include_adult=false&language=en-US&page=${page}&api_key=${
          import.meta.env.VITE_MOVIES_API_KEY
        }`,
    }),
  }),
});

export interface MovieQueryParams {
  name: string;
  page?: number;
}

// Export hooks for usage in functional components, which are
// auto-generated based on the defined endpoints
export const { useGetMoviesQuery, useLazyGetMoviesQuery } = moviesApi;
