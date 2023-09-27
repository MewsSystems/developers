import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react"
import humps from "humps"

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
      query: ({ searchValue, page }) => ({
        url: `/search/movie?query=${searchValue}&page=${page}`,
        responseHandler(response) {
          return response.json().then((data) => {
            return {
              page: data.page,
              results: humps.camelizeKeys(data.results),
              total_pages: data.total_pages,
              total_results: data.total_results,
            }
          })
        },
      }),
    }),
    getMovieDetails: builder.query<Movie, number>({
      query: (id) => ({
        url: `/movie/${id}`,
        responseHandler(response) {
          return response.json().then((data) => {
            return humps.camelizeKeys(data)
          })
        },
      }),
    }),
  }),
})

export const { useGetMoviesQuery, useGetMovieDetailsQuery } = movieApi
