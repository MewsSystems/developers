import { Movie } from '@/types'
import React from 'react'
import { getMoviePosterPath } from '@/utils'
import { Divider, Grid, Rating, Stack, styled, Typography } from '@mui/material'

interface MovieDetailsProps {
  movie: Movie
}

const posterWidth = 500

const StyledImage = styled('img')`
  width: 100%;
  max-width: ${posterWidth}px;
`

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
      <Grid item md={4} xs={12}>
        <StyledImage
          src={getMoviePosterPath(posterWidth, poster_path)}
          alt={title}
        />
      </Grid>
      <Grid item md={8} xs={12}>
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
            <Typography
              color="textSecondary"
              variant={'body2'}
              data-testid={'voteCount'}
            >
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
