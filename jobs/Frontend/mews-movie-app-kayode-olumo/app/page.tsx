"use client"

import { useState, useEffect } from "react"
import type { Movie, MoviesResponse } from "@/lib/types"
import { searchMovies } from "@/lib/services/movieService"
import { useDebounce } from "@/hooks/use-debounce"
import { MovieCard } from "@/components/MovieCard"
import { SearchBar } from "@/components/SearchBar"
import { ScrollToTop } from "@/components/ScrollToTop"
import { Button } from "@/components/ui/button"
import { Loader2 } from "lucide-react"

export default function HomePage() {
  const [searchQuery, setSearchQuery] = useState("")
  const [movies, setMovies] = useState<Movie[]>([])
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [loading, setLoading] = useState(false)

  const debouncedSearch = useDebounce(searchQuery, 500)

  const fetchMovies = async (searchTerm: string, pageNum: number, append = false) => {
    setLoading(true)
    try {
      const data: MoviesResponse = await searchMovies(searchTerm, pageNum)
      setMovies(append ? [...movies, ...data.results] : data.results)
      setTotalPages(data.total_pages)
    } catch (error) {
      console.error("Error fetching movies:", error)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    setPage(1)
    fetchMovies(debouncedSearch, 1, false)
  }, [debouncedSearch])

  const loadMore = () => {
    const nextPage = page + 1
    setPage(nextPage)
    fetchMovies(debouncedSearch, nextPage, true)
  }

  return (
    <div className="min-h-screen bg-background">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6 sm:py-8 lg:py-12">
        {/* Header */}
        <header className="mb-8 sm:mb-12">
          <h1 className="text-4xl sm:text-5xl lg:text-6xl font-bold mb-4 sm:mb-6 tracking-tight">flixDB</h1>
          <p className="text-base sm:text-lg text-muted-foreground max-w-2xl leading-relaxed">
            {searchQuery ? "Search results" : "Discover popular movies"}
          </p>
        </header>

        {/* Search Bar */}
        <div className="mb-8 sm:mb-12">
          <SearchBar value={searchQuery} onChange={setSearchQuery} placeholder="Search for movies..." />
        </div>

        {/* Section Title */}
        <div className="mb-6 sm:mb-8">
          <h2 className="text-2xl sm:text-3xl lg:text-4xl font-bold tracking-tight">
            {searchQuery ? "Search Results" : "Now Showing"}
          </h2>
        </div>

        {/* Movies Grid */}
        {loading && movies.length === 0 ? (
          <div className="flex items-center justify-center py-20">
            <Loader2 className="h-8 w-8 animate-spin text-muted-foreground" />
          </div>
        ) : movies.length === 0 ? (
          <div className="text-center py-20">
            <p className="text-muted-foreground">No movies found</p>
          </div>
        ) : (
          <>
            <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 xl:grid-cols-6 gap-5 sm:gap-6 lg:gap-8 mb-8">
              {movies.map((movie) => (
                <MovieCard key={movie.id} movie={movie} />
              ))}
            </div>

            {/* Load More Button */}
            {page < totalPages && (
              <div className="flex justify-center mt-8 sm:mt-12">
                <Button onClick={loadMore} disabled={loading} size="lg" className="min-w-[140px]">
                  {loading ? (
                    <>
                      <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                      Loading...
                    </>
                  ) : (
                    "Load More"
                  )}
                </Button>
              </div>
            )}
          </>
        )}
      </div>

      {/* Scroll To Top Button */}
      <ScrollToTop />
    </div>
  )
}
