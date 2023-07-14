import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { BaseQueryFn } from "src/baseQuery";
import type { Movie } from "src/store/types/Movie";

// Define a service using a base URL and expected endpoints

const baseQuery: BaseQueryFn<string, unknown> = fetchBaseQuery({
  baseUrl: "https://api.themoviedb.org/3/search/",
});

export const moviesApi = createApi({
  reducerPath: "moviesApi",
  baseQuery,
  endpoints: (builder) => ({
    getMovies: builder.query<Movie, string>({
      query: (name) =>
        `movie?query=${name}&include_adult=false&language=en-US&page=1&api_key=${
          import.meta.env.VITE_MOVIES_API_KEY
        }`,
    }),
  }),
});

// Export hooks for usage in functional components, which are
// auto-generated based on the defined endpoints
export const { useGetMoviesQuery } = moviesApi;
