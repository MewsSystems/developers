import styled from "styled-components"
import type { Movie } from "../types/movie"
import { MovieCard } from "./MovieCard"
import { MovieCardSkeleton } from "./SkeletonCard"

const Grid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: ${({ theme }) => theme.spacing.lg};
  margin-top: ${({ theme }) => theme.spacing.xl};
`

interface MovieGridProps {
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
