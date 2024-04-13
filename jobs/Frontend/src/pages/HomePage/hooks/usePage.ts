import { HomePageViewProps, UseHomeHookProps } from '../types'
import { useMovieSearch } from './useMovieSearch'

export const usePage = (props: UseHomeHookProps): HomePageViewProps => {
    const movieSearch = useMovieSearch()

    return { movieSearch }
}
