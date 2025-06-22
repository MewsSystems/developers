import styled from "styled-components"
import type { Movie } from "../types/movie"
import { MovieCard } from "./MovieCard"

const Grid = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: ${({ theme }) => theme.spacing.lg};
  margin-top: ${({ theme }) => theme.spacing.xl};
`

interface MovieGridProps {
  movies: Movie[]
}

export const MovieGrid = ({ movies }: MovieGridProps) => {
  return (
    <Grid>
      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </Grid>
  )
}
