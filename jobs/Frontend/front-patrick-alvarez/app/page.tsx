'use client'
import { InfiniteMovieList } from '@/components/InfiniteMovieList'
import { MovieListSkeletons } from '@/components/MovieListSkeletons'
import { MovieSearchEmptyState } from '@/components/MovieSearchEmptyState'
import SearchBar from '@/components/SearchBar'
import config from '@/const'
import { MoviesContext } from '@/provider/MoviesProvider'
import { useContext, useMemo } from 'react'
import { useDebouncedCallback } from 'use-debounce'

export default function Home() {
    const { setSearchTerm, moviesInfiniteQuery } = useContext(MoviesContext)
    const { data, isLoading, isSuccess } = moviesInfiniteQuery
    const movies = useMemo(
        () => data?.pages.flatMap((page) => page.results) || [],
        [data?.pages],
    )

    const handleSearch = useDebouncedCallback(
        async (query: string) => {
            setSearchTerm(query)
        },
        config.SEARCH_TYPING_DEBOUNCE_DELAY,
        { leading: false, trailing: true },
    )

    return (
        <main className="flex h-full w-full flex-col items-center gap-y-8">
            <h1 className="text-4xl font-bold">Movie Search</h1>
            <SearchBar handleSearch={handleSearch} />
            {isLoading ? (
                <MovieListSkeletons />
            ) : isSuccess && movies.length > 0 ? (
                <InfiniteMovieList
                    movies={movies}
                    isFetchingNextPage={moviesInfiniteQuery.isFetchingNextPage}
                    fetchNextPage={moviesInfiniteQuery.fetchNextPage}
                />
            ) : (
                <MovieSearchEmptyState />
            )}
        </main>
    )
}
