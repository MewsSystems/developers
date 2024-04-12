import { MovieSearchProps } from '../../../api/movies/types'

export interface UseHomeHookProps {}

export interface HomePageViewProps {
    submitSearchedTitle: (movieTitle: string) => void
}

export type MovieSearchData = Pick<MovieSearchProps, 'movieTitle'>
