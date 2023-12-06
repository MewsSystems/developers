import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react"

export const apiSlice = createApi({
  reducerPath: "api",
  baseQuery: fetchBaseQuery({ baseUrl: "https://api.themoviedb.org/3" }),
  endpoints: (builder) => ({
    getMovies: builder.query({
      query: (params) => {
        const url = `/search/movie?query=${params.term}&include_adult=false&language=en-US&page=${params.page}`
        const headers = {
          accept: "application/json",
          Authorization: `Bearer ${process.env.TMDB_ACCESS_TOKEN}`,
        }
        return { url: url, method: "GET", headers: headers }
      },
    }),
    getMovie: builder.query({
      query: (params) => {
        const url = `/movie/${params.id}`
        const headers = {
          accept: "application/json",
          Authorization: `Bearer ${process.env.TMDB_ACCESS_TOKEN}`,
        }
        return { url: url, method: "GET", headers: headers }
      },
    }),
  }),
})

export const { useGetMoviesQuery, useGetMovieQuery } = apiSlice
