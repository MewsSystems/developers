import { api } from '../../core/api/apiSlice';
import {
    MovieSearchFormValues,
    MovieSearchResponse,
} from '../types/MovieSearchTypes';

export const moviesApiSlice = api.injectEndpoints({
    endpoints: (builder) => ({
        getMoviesBySearch: builder.query<
            MovieSearchResponse,
            MovieSearchFormValues
        >({
            query: ({ searchQuery, pageNumber }) =>
                `/search/movie?query=${searchQuery}&include_adult=false&language=en-US&page=${pageNumber}`,
        }),

        getDiscoverMovies: builder.query<MovieSearchResponse, void>({
            query: () =>
                '/discover/movie?include_adult=false&include_video=false&language=en-US&page=1&sort_by=popularity.desc&year=2024',
        }),
    }),
});

export const { useLazyGetMoviesBySearchQuery, useLazyGetDiscoverMoviesQuery } =
    moviesApiSlice;
