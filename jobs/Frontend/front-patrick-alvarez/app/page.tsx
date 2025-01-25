'use client'
import MovieCard from '@/components/MovieCard'
import SearchBar from '@/components/SearchBar'
import { MoviesContext } from '@/provider/MoviesProvider'
import { useContext, useMemo } from 'react'
import { useDebouncedCallback } from 'use-debounce'

export default function Home() {
    const { setSearchTerm, moviesInfiniteQuery } =
        useContext(MoviesContext)

    const { data, isLoading, isSuccess } = moviesInfiniteQuery

    const movies = useMemo(() => data?.pages.flat() || [], [data?.pages])

    const handleSearch = useDebouncedCallback(
        async (query: string) => {
            setSearchTerm(query)
        },
        1000,
        { leading: false, trailing: true },
    )

    return (
        <main className="flex h-full flex-col items-center gap-8">
            <h1 className="text-4xl font-bold">Movie Search</h1>
            <SearchBar handleSearch={handleSearch} />
            <section className="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3">
                {isLoading ? (
                    <div>Loading...</div>
                ) : (
                    isSuccess && (
                        movies.map((movie) => (
                            <MovieCard key={movie.id} movie={movie} />
                        ))
                    )
                )}
            </section>
        </main>
    )
}
