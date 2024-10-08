"use client"
import { use, useMemo } from "react"
import { useDebounce } from "use-debounce"

import { Button } from "@/app/components/ui"
import { SearchContext } from "@/app/providers/MovieSearchProvider"

import { useSearchMovies } from "../hooks/useSearchMovies"

import { MovieCard } from "./MovieCard"
import { MovieCardGrid } from "./MovieCardGrid"
import { MovieCardSkeleton } from "./MovieCardSkeleton"

export const MoviesContainer = () => {
  const defaultSearch = "marvel"
  const { search } = use(SearchContext)
  const [debouncedSearch] = useDebounce(search, 1000)

  const effectiveSearch = useMemo(() => {
    return debouncedSearch.trim() || defaultSearch
  }, [debouncedSearch])

  const { data, isLoading, fetchNextPage, hasNextPage } =
    useSearchMovies(effectiveSearch)

  if (isLoading) {
    return (
      <MovieCardGrid>
        {Array.from({ length: 10 }).map((_, index) => (
          <MovieCardSkeleton key={index} />
        ))}
      </MovieCardGrid>
    )
  }

  const movies =
    data?.pages
      .flat()
      .map((page) => page.results)
      .flat() || []

  if (movies.length === 0) {
    return <p>No movies found</p>
  }

  return (
    <>
      <MovieCardGrid>
        {movies.map((movie) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </MovieCardGrid>
      {hasNextPage && (
        <Button className="mx-auto" onClick={() => fetchNextPage()}>
          Load More Movies
        </Button>
      )}
    </>
  )
}
