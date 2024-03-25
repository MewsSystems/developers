import React from 'react'
import { Movie } from '@/types'
import { ListItem, useTheme } from '@mui/material'
import { FixedSizeList, ListChildComponentProps } from 'react-window'
import { MovieListItem } from '@/components'
import { getMoviePosterDimensions } from '@/utils'

interface MoviesListProps {
  movies: Movie[]
}

const posterWidth = 50
const { height: posterHeight } = getMoviePosterDimensions(posterWidth)

export const MovieList = ({ movies }: MoviesListProps) => {
  const theme = useTheme()
  const paddingItem = parseInt(theme.spacing(2), 10)

  const renderRow = (props: ListChildComponentProps) => {
    const { index, style } = props
    const { id } = movies[index]

    return (
      <ListItem style={style} key={id} component="div" disablePadding>
        <MovieListItem movie={movies[index]} />
      </ListItem>
    )
  }

  return (
    <FixedSizeList
      height={400}
      width={'100%'}
      itemSize={posterHeight + paddingItem}
      itemCount={movies.length}
      overscanCount={5}
    >
      {renderRow}
    </FixedSizeList>
  )
}
