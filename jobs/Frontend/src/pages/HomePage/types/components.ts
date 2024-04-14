import type { MovieSearch } from '.'

export type HomeSearchContentProps = Omit<
    MovieSearch,
    | 'submitSearchedTitle'
    | 'currentSearchTitle'
    | 'searchInputRef'
    | 'clearTitle'
> & {
    prefetchMovieData: (movie_id: number) => Promise<void>
}

export type UseHomeSearchHook = Omit<
    HomeSearchContentProps,
    'prefetchMovieData'
>
