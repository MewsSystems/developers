import {createApi, fetchBaseQuery} from "@reduxjs/toolkit/query/react";
import { MovieDetail } from "~/features/movieDetail/types";
import { MovieListQueryResult, MovieListSearchQueryParams } from "~/features/movieList/types";

export const moviesApi = createApi({
  baseQuery: fetchBaseQuery({
    baseUrl: `${process.env.NEXT_PUBLIC_API_BASE_URL}`,
    prepareHeaders: (headers) => {
      headers.set("Content-Type", "application/json;charset=utf-8");
      return headers;
    },
  }),
  tagTypes: ["Movie"],
  //movie endpoints definition
  endpoints: (build) => ({
    getSearchMovies: build.query<MovieListQueryResult, MovieListSearchQueryParams>({
      // find a way to add api_key as default param to all queries
      query: (arg) => ({url: "search/movie", params: {...arg, "api_key": process.env.NEXT_PUBLIC_AUTH_TOKEN}}),
    }),
    getMovieId: build.query<MovieDetail, number>({
      // find a way to add api_key as default param to all queries
      query: (id) => ({url: `movie/${id}`, params: {"api_key": process.env.NEXT_PUBLIC_AUTH_TOKEN}}),
    }),
  })
});