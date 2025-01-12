import type { ChangeEvent, RefObject } from 'react'
import type { MovieSearchCollection } from '../../../api/movies/types'
import type { DebouncedFunc } from 'lodash'

export * from './components'

export interface UseHomeHookProps {}

export interface MovieSearch {
    currentSearchTitle: string
    clearTitle: () => void
    searchInputRef: RefObject<HTMLInputElement>
    submitSearchedTitle: DebouncedFunc<(movieTitle: string) => void>
    searchData: MovieSearchCollection | undefined
    handleChangePage: (event: ChangeEvent<unknown>, value: number) => void
    isLoading: boolean
    isError: boolean
}

export interface HomePageViewProps {
    movieSearch: MovieSearch
}
