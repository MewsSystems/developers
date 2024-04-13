import { MovieSearch } from '.'

export type HomeSearchContentProps = Omit<MovieSearch, 'submitSearchedTitle'>

export interface UseHomeSearchHook extends HomeSearchContentProps {}
