import { api } from '../../core/api/apiSlice';
import {
    MovieDetailsFormValues,
    MovieDetailsResponse,
} from '../types/MovieDetailsTypes';

export const movieDetailsApiSlice = api.injectEndpoints({
    endpoints: (builder) => ({
        getMovieDetails: builder.query<
            MovieDetailsResponse,
            MovieDetailsFormValues
        >({
            query: ({ movieId }) => `/movie/${movieId}`,
        }),
    }),
});

export const { useGetMovieDetailsQuery } = movieDetailsApiSlice;
