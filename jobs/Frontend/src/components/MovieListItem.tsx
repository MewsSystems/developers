import { Box, ListItemButton, styled, Typography } from '@mui/material'
import { getMoviePosterDimensions, getMoviePosterPath } from '@/utils'
import React from 'react'
import { useNavigate } from 'react-router-dom'
import { Movie } from '@/types'

interface MovieListItemProps {
  movie: Movie
}

const posterWidth = 50
const { height: posterHeight } = getMoviePosterDimensions(posterWidth)

const StyledListItemButton = styled(ListItemButton)(({ theme }) => ({
  padding: theme.spacing(1),
}))

const StyledBox = styled(Box)(({ theme }) => ({
  width: posterWidth + 'px',
  height: posterHeight + 'px',
  marginRight: theme.spacing(2),

  '&.default-img': {
    backgroundColor: 'grey',
  },
}))

export const MovieListItem = ({ movie }: MovieListItemProps) => {
  const navigate = useNavigate()
  const { id, poster_path, title } = movie

  const handleClick = (id: number) => {
    navigate(`/movie/${id}`)
  }

  return (
    <StyledListItemButton onClick={() => handleClick(id)}>
      {poster_path ? (
        <StyledBox
          component="img"
          src={getMoviePosterPath(200, poster_path)}
          alt={title}
        />
      ) : (
        <StyledBox className={'default-img'} />
      )}
      <Typography color="textSecondary" variant="body1">
        {title}
      </Typography>
    </StyledListItemButton>
  )
}
