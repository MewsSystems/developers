import { notFound } from "next/navigation"
import { getImageUrl } from "@/lib/tmdb"
import { fetchMovieDetails } from "@/lib/api/tmdb"
import type { MovieDetail } from "@/lib/types"
import { MoviePoster } from "@/components/MoviePoster"
import { Header } from "@/components/Header"
import { MovieMeta } from "@/components/MovieMeta"
import { FinancialInfo } from "@/components/FinancialInfo"
import { GenresList } from "@/components/GenresList"
import { ProductionCompanies } from "@/components/ProductionCompanies"
import { LanguagesList } from "@/components/LanguagesList"
import { MovieStatus } from "@/components/MovieStatus"
import { Container } from "@/components/Container"

interface MoviePageProps {
  params: Promise<{ id: string }>
}

export default async function MoviePage({ params }: MoviePageProps) {
  const { id } = await params

  let movie: MovieDetail

  try {
    movie = await fetchMovieDetails(id)
    
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
      <Header variant="back" />

      <main>
        <Container className="py-6 sm:py-8 lg:py-12">
          <div className="grid lg:grid-cols-[350px_1fr] gap-6 sm:gap-8 lg:gap-12">
          <div className="mx-auto lg:mx-0 w-full max-w-sm lg:max-w-none">
            <MoviePoster
              src={getImageUrl(movie.poster_path, "w500") || "/placeholder.svg?height=750&width=500"}
              alt={movie.title}
              priority
            />
          </div>

          <div className="space-y-6 sm:space-y-8">
            <div>
              <h1 className="text-3xl sm:text-4xl lg:text-5xl font-bold mb-3 leading-tight">{movie.title}</h1>
              {movie.tagline && <p className="text-base sm:text-lg italic text-muted-foreground">{movie.tagline}</p>}
            </div>

            <MovieMeta
              vote_average={movie.vote_average}
              vote_count={movie.vote_count}
              release_date={movie.release_date}
              runtime={movie.runtime}
            />

            <GenresList genres={movie.genres || []} />

            <div>
              <h3 className="text-lg sm:text-xl font-bold mb-3">Overview</h3>
              <p className="leading-relaxed text-base">{movie.overview}</p>
            </div>

            <FinancialInfo budget={movie.budget} revenue={movie.revenue} />

            <ProductionCompanies companies={movie.production_companies || []} />

            <LanguagesList languages={movie.spoken_languages || []} />

            <MovieStatus status={movie.status} />
          </div>
          </div>
        </Container>
      </main>
    </div>
  )
}
