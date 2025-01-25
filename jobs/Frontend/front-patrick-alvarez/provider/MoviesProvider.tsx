'use client'
import {
    InfiniteData,
    useInfiniteQuery,
    UseInfiniteQueryResult,
} from '@tanstack/react-query'
/* eslint-disable @typescript-eslint/no-unused-vars */

import { fetchMovies } from '@/api'
import cacheConfig from '@/const/cache'
import { Movie } from '@/types/Movie'
import { createContext, PropsWithChildren, useState } from 'react'

interface MovieResponse {
    results: Movie[]
    page: number
    total_pages: number
    total_results: number
}

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
