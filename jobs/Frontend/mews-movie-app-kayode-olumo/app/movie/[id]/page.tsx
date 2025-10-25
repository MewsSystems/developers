import { notFound } from "next/navigation"
import Link from "next/link"
import { getImageUrl } from "@/lib/tmdb"
import { fetchMovieDetails } from "@/lib/api/tmdb"
import { formatCurrency, formatRuntime, formatYear } from "@/lib/utils/formatters"
import type { MovieDetail } from "@/lib/types"
import { Button } from "@/components/ui/button"
import { MoviePoster } from "@/components/MoviePoster"
import { ArrowLeft, Calendar, Clock, Star, DollarSign } from "lucide-react"

interface MoviePageProps {
  params: Promise<{ id: string }>
}

export default async function MoviePage({ params }: MoviePageProps) {
  const { id } = await params

  let movie: MovieDetail

  try {
    movie = await fetchMovieDetails(id)
    
    // Validate that we have the essential data
    if (!movie || !movie.id || !movie.title) {
      console.error('Invalid movie data received:', movie)
      notFound()
    }
  } catch (error) {
    console.error('Error fetching movie details:', error)
    notFound()
  }


  return (
    <div className="min-h-screen bg-background">
      {/* Header */}
      <header className="border-b bg-background sticky top-0 z-10 backdrop-blur-sm bg-background/95">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <Link href="/">
            <Button variant="ghost" className="gap-2">
              <ArrowLeft className="h-4 w-4" />
              <span className="hidden sm:inline">Back to Movies</span>
              <span className="sm:hidden">Back</span>
            </Button>
          </Link>
        </div>
      </header>

      {/* Movie Details */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6 sm:py-8 lg:py-12">
        <div className="grid lg:grid-cols-[350px_1fr] gap-6 sm:gap-8 lg:gap-12">
          {/* Poster */}
          <div className="mx-auto lg:mx-0 w-full max-w-sm lg:max-w-none">
            <MoviePoster
              src={getImageUrl(movie.poster_path, "w500") || "/placeholder.svg?height=750&width=500"}
              alt={movie.title}
              priority
            />
          </div>

          {/* Info */}
          <div className="space-y-6 sm:space-y-8">
            <div>
              <h1 className="text-3xl sm:text-4xl lg:text-5xl font-bold mb-3 leading-tight">{movie.title}</h1>
              {movie.tagline && <p className="text-base sm:text-lg italic text-muted-foreground">{movie.tagline}</p>}
            </div>

            {/* Meta Info */}
            <div className="flex flex-wrap gap-4 sm:gap-6 text-sm">
              <div className="flex items-center gap-2">
                <Star className="h-4 w-4 fill-primary text-primary" />
                <span className="font-semibold">{movie.vote_average.toFixed(1)}</span>
                <span className="text-muted-foreground">({movie.vote_count.toLocaleString()} votes)</span>
              </div>

                  {movie.release_date && (
                    <div className="flex items-center gap-2">
                      <Calendar className="h-4 w-4 text-muted-foreground" />
                      <span>{formatYear(movie.release_date)}</span>
                    </div>
                  )}

              {movie.runtime && movie.runtime > 0 && (
                <div className="flex items-center gap-2">
                  <Clock className="h-4 w-4 text-muted-foreground" />
                  <span>{formatRuntime(movie.runtime)}</span>
                </div>
              )}
            </div>

            {/* Genres */}
            {movie.genres && movie.genres.length > 0 && (
              <div>
                <h3 className="text-lg sm:text-xl font-bold mb-3">Genres</h3>
                <div className="flex flex-wrap gap-2">
                  {movie.genres.map((genre) => (
                    <span key={genre.id} className="px-4 py-2 bg-secondary border-2 rounded-full text-sm font-medium">
                      {genre.name}
                    </span>
                  ))}
                </div>
              </div>
            )}

            {/* Overview */}
            <div>
              <h3 className="text-lg sm:text-xl font-bold mb-3">Overview</h3>
              <p className="leading-relaxed text-base">{movie.overview}</p>
            </div>

            {/* Financial Info */}
            {((movie.budget && movie.budget > 0) || (movie.revenue && movie.revenue > 0)) && (
              <div className="grid sm:grid-cols-2 gap-4">
                {movie.budget && movie.budget > 0 && (
                  <div className="p-4 sm:p-6 border-2 rounded-xl overflow-hidden shadow-lg">
                    <div className="flex items-center gap-2 mb-2">
                      <DollarSign className="h-5 w-5 text-muted-foreground" />
                      <span className="text-sm font-semibold uppercase tracking-wide">Budget</span>
                    </div>
                    <p className="text-xl sm:text-2xl font-bold">{formatCurrency(movie.budget)}</p>
                  </div>
                )}

                {movie.revenue && movie.revenue > 0 && (
                  <div className="p-4 sm:p-6 border-2 rounded-xl overflow-hidden shadow-lg">
                    <div className="flex items-center gap-2 mb-2">
                      <DollarSign className="h-5 w-5 text-muted-foreground" />
                      <span className="text-sm font-semibold uppercase tracking-wide">Revenue</span>
                    </div>
                    <p className="text-xl sm:text-2xl font-bold">{formatCurrency(movie.revenue)}</p>
                  </div>
                )}
              </div>
            )}

            {/* Production Companies */}
            {movie.production_companies && movie.production_companies.length > 0 && (
              <div>
                <h3 className="text-lg sm:text-xl font-bold mb-3">Production Companies</h3>
                <div className="flex flex-wrap gap-3">
                  {movie.production_companies.map((company) => (
                    <span key={company.id} className="text-sm bg-secondary px-3 py-1 rounded-lg">
                      {company.name}
                    </span>
                  ))}
                </div>
              </div>
            )}

            {/* Languages */}
            {movie.spoken_languages && movie.spoken_languages.length > 0 && (
              <div>
                <h3 className="text-lg sm:text-xl font-bold mb-2">Languages</h3>
                <p className="text-sm text-muted-foreground">
                  {movie.spoken_languages.map((lang) => lang.name).join(", ")}
                </p>
              </div>
            )}

            {/* Status */}
            {movie.status && (
              <div>
                <h3 className="text-lg sm:text-xl font-bold mb-2">Status</h3>
                <p className="text-sm text-muted-foreground">{movie.status}</p>
              </div>
            )}
          </div>
        </div>
      </main>
    </div>
  )
}
