import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react"
import { Movie, MovieListQueryResponse } from "../types"

interface GetMoviesQueryParams {
  searchValue: string
  page: number
}

export const movieApi = createApi({
  reducerPath: "movies",
  baseQuery: fetchBaseQuery({
    baseUrl: "https://api.themoviedb.org/3/",
    prepareHeaders: (headers) => {
      headers.set(
        "authorization",
        `Bearer ${import.meta.env["VITE_API_TOKEN"]}`,
      )
      return headers
    },
  }),
  endpoints: (builder) => ({
    getMovies: builder.query<MovieListQueryResponse, GetMoviesQueryParams>({
      query: ({ searchValue, page }) =>
        `/search/movie?query=${searchValue}&page=${page}`,
    }),
    getMovieDetails: builder.query<Movie, number>({
      query: (id) => `/movie/${id}`,
    }),
  }),
})

export const { useGetMoviesQuery, useGetMovieDetailsQuery } = movieApi
