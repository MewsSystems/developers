'use client'

import {
    InfiniteData,
    useInfiniteQuery,
    UseInfiniteQueryResult,
} from '@tanstack/react-query'

import { fetchMovies } from '@/api'
import cacheConfig from '@/const/cache'
import { MovieResponse } from '@/types/MovieResponse'
import { createContext, PropsWithChildren, useState } from 'react'

interface MoviesContextValue {
    searchTerm: string | undefined
    setSearchTerm: (query: string) => void
    moviesInfiniteQuery: UseInfiniteQueryResult<
        InfiniteData<MovieResponse>,
        Error
    >
}

export const MoviesContext = createContext<MoviesContextValue>({
    searchTerm: undefined,
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    setSearchTerm: (searchTerm: string) => {},
    moviesInfiniteQuery: {} as UseInfiniteQueryResult<
        InfiniteData<MovieResponse>,
        Error
    >,
})

export default MoviesContext

export const MoviesProvider = ({ children }: PropsWithChildren) => {
    const [searchTerm, setSearchTerm] = useState<string | undefined>(undefined)

    const moviesInfiniteQuery = useInfiniteQuery({
        queryKey: [cacheConfig.BASE_KEYS.MOVIES, searchTerm],
        queryFn: ({ pageParam }) =>
            fetchMovies({ page: pageParam, query: searchTerm }),
        getNextPageParam: (lastPage, pages) => {
            return lastPage.results.length ? pages.length + 1 : undefined
        },
        initialPageParam: 1,
    })

    return (
        <MoviesContext.Provider
            value={{
                searchTerm,
                setSearchTerm,
                moviesInfiniteQuery,
            }}
        >
            {children}
        </MoviesContext.Provider>
    )
}
