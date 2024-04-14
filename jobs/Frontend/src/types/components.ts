import type { StackProps } from '@mui/material'
import type { MovieSearchItem } from '.'

export type MovieThumbnailProps = MovieSearchItem & Omit<StackProps, 'id'>
