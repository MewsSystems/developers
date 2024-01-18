// Or from '@reduxjs/toolkit/query' if not using the auto-generated hooks
import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

// initialize an empty api service that we'll inject endpoints into later as needed
export const TMDBApiSlice = createApi({
  reducerPath: 'tmdb-api',
  baseQuery: fetchBaseQuery({
    baseUrl: 'https://api.themoviedb.org',
    prepareHeaders: (headers) => {
      headers.set(
        'Authorization',
        `Bearer ${process.env.NEXT_PUBLIC_ENV_TMDBAPI_TOKEN}`
      );
      headers.set('Content-Type', 'application/json');
      headers.set('Accept', 'application/json');

      return headers;
    },
  }),
  endpoints: () => ({}),
})
