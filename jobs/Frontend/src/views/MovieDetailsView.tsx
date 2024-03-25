import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { Movie } from '@/types'
import { movieService } from '@/services'
import { MovieDetails } from '@/components'

export const MovieDetailsView = () => {
  const { id } = useParams()
  const [movie, setMovie] = useState<Movie | null>(null)

  useEffect(() => {
    const idFormatted = parseInt(id)

    movieService.getMovieDetails(idFormatted).then((response) => {
      setMovie(response)
    })
  }, [])

  return movie && <MovieDetails movie={movie} />
}
