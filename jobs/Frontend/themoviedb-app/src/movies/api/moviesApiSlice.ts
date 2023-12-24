import { api } from '../../core/api/apiSlice';
import {
    MovieSearchFormValues,
    MovieSearchResponse,
} from '../models/MovieSearchModels';

export const moviesApiSlice = api.injectEndpoints({
    endpoints: (builder) => ({
        getMoviesBySearch: builder.query<
            MovieSearchResponse,
            MovieSearchFormValues
        >({
            query: ({ searchQuery, pageNumber }) =>
                `/search/movie?query=${searchQuery}&include_adult=false&language=en-US&page=${pageNumber}`,
        }),
    }),
});

export const { useLazyGetMoviesBySearchQuery } = moviesApiSlice;
