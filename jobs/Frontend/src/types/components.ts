import type { StackProps } from '@mui/material'
import type { MovieSearchItem } from '.'
import { HTMLAttributes } from 'react'
import { MovieSearchCollection } from '../api/movies/types'

export type MovieThumbnailProps = MovieSearchItem & Omit<StackProps, 'id'>

export type MoviePosterProps = Pick<MovieSearchItem, 'poster_path' | 'title'> &
    HTMLAttributes<HTMLImageElement>

export interface PosterContentProps {
    searchData?: MovieSearchCollection
    limitMovies?: boolean
    noDataText: string
    errorText: string
    isLoading: boolean
    isError: boolean
    prefetchFunction: (movie_id: number) => Promise<void>
}
