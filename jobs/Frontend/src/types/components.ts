import type { StackProps } from '@mui/material'
import type { MovieSearchItem } from '.'
import { HTMLAttributes } from 'react'

export type MovieThumbnailProps = MovieSearchItem & Omit<StackProps, 'id'>

export type MoviePosterProps = Pick<MovieSearchItem, 'poster_path' | 'title'> &
    HTMLAttributes<HTMLImageElement>
