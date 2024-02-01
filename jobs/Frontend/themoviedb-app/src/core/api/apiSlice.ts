import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';

export const api = createApi({
    reducerPath: 'api',
    baseQuery: fetchBaseQuery({
        baseUrl: 'https://api.themoviedb.org/3',
        prepareHeaders: (headers) => {
            headers.set(
                'Authorization',
                `Bearer ${import.meta.env.VITE_TMDB_ACCESS_TOKEN}`
            );
            headers.set('Content-Type', 'application/json');
            headers.set('Accept', 'application/json');

            return headers;
        },
    }),
    endpoints: (builder) => ({}),
});
