"use client"

import { useState, useEffect } from "react"
import type { Movie, MoviesResponse } from "@/types"
import { searchMovies } from "@/lib/services/movieService"
import { useDebounce } from "@/hooks/use-debounce"
import { DEFAULT_DEBOUNCE_MS } from "@/lib/constants"
import { MovieCard } from "@/components/MovieCard"
import { SearchBar } from "@/components/SearchBar"
import { ScrollToTop } from "@/components/ScrollToTop"
import { Header } from "@/components/Header"
import { SectionTitle } from "@/components/SectionTitle"
import { Container } from "@/components/Container"
import { Button } from "@/components/ui/button"
import { Loader2 } from "lucide-react"

export default function HomePage() {
  const [searchQuery, setSearchQuery] = useState("")
  const [movies, setMovies] = useState<Movie[]>([])
  const [page, setPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [loading, setLoading] = useState(false)

  const debouncedSearch = useDebounce(searchQuery, DEFAULT_DEBOUNCE_MS)

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
      <Container className="py-6 sm:py-8 lg:py-12">
        <Header 
          title="flixDB" 
          subtitle={searchQuery ? "Search results" : "Discover popular movies"} 
        />

        <div className="mb-8 sm:mb-12">
          <SearchBar value={searchQuery} onChange={setSearchQuery} placeholder="Search for movies..." />
        </div>

        <div className="mb-6 sm:mb-8">
          <SectionTitle>
            {searchQuery ? "Search Results" : "Now Showing"}
          </SectionTitle>
        </div>

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
      </Container>

      <ScrollToTop />
    </div>
  )
}
