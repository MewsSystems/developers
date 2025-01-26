'use client'

import { useQuery, UseQueryResult } from '@tanstack/react-query'

import { fetchMovie } from '@/api'
import { Loading } from '@/components/Loading'
import { MovieDetailEmptyState } from '@/components/MovieDetailEmptyState'
import cacheConfig from '@/const/cache'
import Movie from '@/types/Movie'
import { createContext, PropsWithChildren } from 'react'

interface MoviesContextValue {
    movieQuery: UseQueryResult<Movie, Error>
}

export const MovieDetailContext = createContext<MoviesContextValue>({
    movieQuery: {
        data: undefined,
        isLoading: false,
        isError: false,
    } as UseQueryResult<Movie, Error>,
})

export default MovieDetailContext

interface MovieDetailProviderProps extends PropsWithChildren {
    id: string
}

export const MovieDetailProvider = ({
    id,
    children,
}: MovieDetailProviderProps) => {
    const movieQuery = useQuery({
        queryKey: [cacheConfig.BASE_KEYS.MOVIE, id],
        queryFn: () => fetchMovie(id),
        enabled: !!id,
    })

    return (
        <MovieDetailContext.Provider value={{ movieQuery }}>
            {movieQuery.isLoading && <Loading />}
            {movieQuery.isError && <MovieDetailEmptyState />}
            {movieQuery.isSuccess && children}
        </MovieDetailContext.Provider>
    )
}
