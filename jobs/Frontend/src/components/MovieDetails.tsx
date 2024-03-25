import { Movie } from '@/types'
import React from 'react'
import { getMoviePosterPath } from '@/utils'
import { Typography } from '@mui/material'

interface MovieDetailsProps {
  movie: Movie
}

export const MovieDetails = ({ movie }: MovieDetailsProps) => {
  const { title, overview, release_date, poster_path } = movie

  return (
    <div>
      <Typography color="textSecondary" variant={'h1'}>
        {title}
      </Typography>
      <Typography color="textSecondary" variant={'body2'}>
        {overview}
      </Typography>
      <Typography color="textSecondary" variant={'body2'}>
        {release_date}
      </Typography>
      <img src={getMoviePosterPath(200, poster_path)} />
    </div>
  )
}
