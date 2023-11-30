import { ElementOf } from "@/utilities/types";
import { operations } from "./generated/types";
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

const api_key = process.env.NEXT_PUBLIC_MOVIE_DB_API_KEY;
const baseUrl = "https://api.themoviedb.org/3/";

type SearchMovieResults =
    operations["search-movie"]["responses"]["200"]["content"]["application/json"];
type SearchMovieArgs = { query: string; page: number };

type MovieDetailResult =
    operations["movie-details"]["responses"]["200"]["content"]["application/json"];
type MovieDetailArgs = number;

export type SearchMovie = ElementOf<SearchMovieResults["results"]>;

export const api = createApi({
    baseQuery: fetchBaseQuery({ baseUrl }),
    endpoints: (builder) => ({
        getMovies: builder.query<SearchMovieResults, SearchMovieArgs>({
            query: ({ query, page }) => ({
                url: `search/movie`,
                params: { api_key, query, page },
            }),
        }),
        getMovieDetail: builder.query<MovieDetailResult, MovieDetailArgs>({
            query: (id: number) => `movie/${id}?api_key=${api_key}`,
        }),
    }),
});

export const { useGetMoviesQuery, useGetMovieDetailQuery } = api;
