import { Movie } from '@/types'
import React from 'react'
import { getMoviePosterPath } from '@/utils'
import { Divider, Grid, Rating, Stack, Typography } from '@mui/material'

interface MovieDetailsProps {
  movie: Movie
}

export const MovieDetails = ({ movie }: MovieDetailsProps) => {
  const {
    title,
    overview,
    release_date,
    poster_path,
    vote_average,
    vote_count,
  } = movie
  const formattedDate = new Date(release_date).toLocaleDateString()
  const formattedVoteAverage = vote_average / 2

  return (
    <Grid container>
      <Grid item xs={4}>
        <img src={getMoviePosterPath(500, poster_path)} />
      </Grid>
      <Grid item xs={8}>
        <Stack spacing={1}>
          <Typography color="textSecondary" variant={'h2'} component={'h1'}>
            {title}
          </Typography>
          <Divider />
          <Typography color="textSecondary" variant={'body2'}>
            {formattedDate}
          </Typography>
          <Stack direction={'row'} alignItems={'center'} spacing={1}>
            <Rating value={formattedVoteAverage} readOnly />
            <Typography color="textSecondary" variant={'body2'} data-testid={'voteCount'}>
              ({vote_count})
            </Typography>
          </Stack>
          <Divider />
          <Typography color="textSecondary" variant={'body2'}>
            {overview}
          </Typography>
        </Stack>
      </Grid>
    </Grid>
  )
}
