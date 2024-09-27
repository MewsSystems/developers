import type { FC } from 'react'
import type { UseMovieDetailHookProps } from './types'
import { wrap } from '../../utils'
import { usePage } from './hooks'
import { MovieDetailView } from './MovieDetailView'

export const MovieDetailPage: FC<UseMovieDetailHookProps> = wrap(
    MovieDetailView,
    usePage,
)
