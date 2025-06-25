import { MovieCard } from "@/components/MovieCard"
import { MovieCardSkeleton } from "@/components/MovieCardSkeleton"
import type { Movie } from "@/types/movie"
import { Grid } from "./MovieGrid.styles"

export interface MovieGridProps {
  movies?: Movie[]
  isLoading?: boolean
  skeletonCount?: number
}

export const MovieGrid = ({ movies, isLoading = false, skeletonCount = 8 }: MovieGridProps) => {
  if (isLoading) {
    return (
      <Grid>
        {Array.from({ length: skeletonCount }, () => (
          <MovieCardSkeleton key={crypto.randomUUID()} />
        ))}
      </Grid>
    )
  }

  if (!movies || movies.length === 0) {
    return null
  }

  return (
    <Grid>
      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </Grid>
  )
}
