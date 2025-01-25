import { Movie } from '@/types/Movie'
import { useEffect, useRef } from 'react'
import MovieCard from './MovieCard'

interface InfiniteMovieListProps {
    movies: Movie[]
    isFetchingNextPage: boolean
    fetchNextPage: () => void
}

export const InfiniteMovieList = ({
    movies,
    isFetchingNextPage,
    fetchNextPage,
}: InfiniteMovieListProps) => {
    const observerTarget = useRef<HTMLDivElement>(null)

    useEffect(() => {
        const observer = new IntersectionObserver(
            (entries) => {
                if (entries[0].isIntersecting && !isFetchingNextPage) {
                    fetchNextPage()
                }
            },
            { threshold: 0.1 },
        )

        if (observerTarget.current) {
            observer.observe(observerTarget.current)
        }

        return () => observer.disconnect()
    }, [fetchNextPage, isFetchingNextPage])

    return (
        <section className="grid w-full grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
            {movies.map((movie, index) => (
                <MovieCard 
                    key={`${movie.id}-${index}`} 
                    movie={movie} 
                />
            ))}
            <div ref={observerTarget} className="h-10" />
            {isFetchingNextPage && (
                <div className="col-span-full py-4 text-center">
                    Loading more movies...
                </div>
            )}
        </section>
    )
}
