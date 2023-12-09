import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'
import { TMDB_API_KEY as API_KEY } from './secret';
import { Movie, TMDBResponse } from '../types'

export const TMDB_BASE_URL = `https://api.themoviedb.org`;
export const TMDB_IMAGE_BASE_URL = `https://image.tmdb.org/t/p/w500`;
const TMDB_URL_API_URL = `${TMDB_BASE_URL}/3/search/`;

export const movieApi = createApi({
    reducerPath: 'movieApi',
    baseQuery: fetchBaseQuery({ baseUrl: TMDB_URL_API_URL }),
    endpoints: (builder) => ({
        getMovies: builder.query<TMDBResponse, { query: string, page: number }>({
            query: ({ query, page }) => `movie?include_adult=false&language=en-US&api_key=${API_KEY}&query=${query}&page=${page}`,
        }),
    }),
})

export const { useGetMoviesQuery } = movieApi