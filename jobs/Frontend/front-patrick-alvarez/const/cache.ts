import { QueryClientConfig } from '@tanstack/react-query'

const cacheConfig = {
    DEFAULT_QUERY_OPTIONS: {
        queries: {
            refetchOnWindowFocus: false,
            retry: false,
            retryOnMount: false,
            refetchOnMount: true,
            staleTime: 60 * 1000, // 60 seconds
        },
    } satisfies QueryClientConfig['defaultOptions'],
    BASE_KEYS: {
        MOVIES: 'movies',
    },
}

export default cacheConfig
