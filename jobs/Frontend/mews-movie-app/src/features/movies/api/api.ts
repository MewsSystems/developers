import { ElementOf } from "@/utilities/types";
import { operations } from "./generated/types";
import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";

type SearchMovieResults =
    operations["search-movie"]["responses"]["200"]["content"]["application/json"];

type MovieDetailResult =
    operations["movie-details"]["responses"]["200"]["content"]["application/json"];

export const api = createApi({
    baseQuery: fetchBaseQuery({ baseUrl: "https://api.themoviedb.org/3/" }),
    endpoints: (builder) => ({
        getMovies: builder.query<
            SearchMovieResults,
            { query: string; page: number }
        >({
            query: ({ query, page }) => {
                return {
                    url: `search/movie`,
                    params: {
                        api_key: process.env.NEXT_PUBLIC_MOVIE_DB_API_KEY,
                        query,
                        page,
                    },
                };
            },
        }),
        getMovieDetail: builder.query<MovieDetailResult, number>({
            query: (movieId: number) => {
                return {
                    url: `movie/${movieId}`,
                    params: {
                        api_key: process.env.NEXT_PUBLIC_MOVIE_DB_API_KEY,
                    },
                };
            },
        }),
    }),
});

export type SearchMovie = ElementOf<SearchMovieResults["results"]>;

export const { useGetMoviesQuery, useGetMovieDetailQuery } = api;
