import { MovieSearchItem } from '../../types'
import { CollectionResponse } from '../types'

export type MovieSearchCollection = CollectionResponse<MovieSearchItem>

export interface MovieSearchProps {
    movieTitle: string
    page?: number
}
