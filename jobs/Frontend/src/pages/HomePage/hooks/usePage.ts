import { HomePageViewProps, UseHomeHookProps } from '../types'
import { useMovieSearch } from './useMovieSearch'

// eslint-disable-next-line @typescript-eslint/no-unused-vars
export const usePage = (props: UseHomeHookProps): HomePageViewProps => {
    const movieSearch = useMovieSearch()

    return { movieSearch }
}
