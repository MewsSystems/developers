"use client"
import { useMovie } from "../hooks/useMovie"

import { MovieDetail } from "./MovieDetail"

export const MovieContainer = ({ movieId }: { movieId: string }) => {
  const { data } = useMovie(movieId)

  if (!data) {
    return null
  }

  return <MovieDetail movie={data} />
}
