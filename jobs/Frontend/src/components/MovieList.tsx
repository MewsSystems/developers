import React from 'react'
import { Movie } from '@/types'
import { ListItem } from '@mui/material'
import { FixedSizeList, ListChildComponentProps } from 'react-window'
import {MovieListItem} from "@/components/MovieListItem";

interface MoviesListProps {
  movies: Movie[]
}

const posterOriginalWidth = 200
const posterWidth = posterOriginalWidth / 4
const posterHeight = posterOriginalWidth / 2.66
const paddingListItem = 8

export const MovieList = ({ movies }: MoviesListProps) => {
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
      itemSize={posterHeight + paddingListItem * 2}
      itemCount={movies.length}
      overscanCount={5}
    >
      {renderRow}
    </FixedSizeList>
  )
}
