import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { Config, Filters, Movies, MovieDetails, ID } from '../../types'

const API_KEY = process.env.NEXT_PUBLIC_TMDB_API_KEY ?? ''
const DEFAULT_PARAMS = { api_key: API_KEY }
const NO_ADULT = { include_adult: 'false' }

// Call with append rather then firing two additional requests
const APPEND_PARAMS = { append_to_response: 'credits,videos' }

export const apiSlice = createApi({
  reducerPath: 'api',
  baseQuery: fetchBaseQuery({
    baseUrl: 'https://api.themoviedb.org/3/',
  }),
  endpoints: (builder) => ({
    getConfig: builder.query<Config, {}>({
      query: () => {
        const queryString = new URLSearchParams(DEFAULT_PARAMS).toString()
        return `configuration?${queryString}`
      },
    }),
    getMovies: builder.query<Movies, Filters>({
      query: (filters) => {
        const queryParams = { ...DEFAULT_PARAMS, ...NO_ADULT, ...filters }
        const queryString = new URLSearchParams(queryParams).toString()
        return (filters.query?.length ?? 0) > 0
          ? `search/movie?${queryString}`
          : `discover/movie?${queryString}`
      },
    }),
    getMovieById: builder.query<MovieDetails, ID>({
      query: ({ id }) => {
        const queryParams = { ...DEFAULT_PARAMS, ...APPEND_PARAMS }
        const queryString = new URLSearchParams(queryParams).toString()
        return `movie/${id}?${queryString}`
      },
    }),
  }),
})

export const { useGetConfigQuery, useGetMoviesQuery, useGetMovieByIdQuery } =
  apiSlice
