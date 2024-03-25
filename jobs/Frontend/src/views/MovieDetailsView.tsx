import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { Movie } from '@/types'
import { getMovieDetails } from '@/services'
import { MovieDetails } from '@/components/MovieDetails'

export const MovieDetailsView = () => {
  const { id } = useParams()
  const [movie, setMovie] = useState<Movie>([])

  useEffect(() => {
    const idFormatted = parseInt(id)
    getMovieDetails(idFormatted).then((response) => {
      setMovie(response)
    })
  }, [])

  return <MovieDetails movie={movie} />
}
