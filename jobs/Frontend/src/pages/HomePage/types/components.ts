import type { MovieSearch } from '.'

export type HomeSearchContentProps = Omit<
    MovieSearch,
    'submitSearchedTitle'
> & {
    prefetchMovieData: (movie_id: number) => Promise<void>
}

export type UseHomeSearchHook = Omit<
    HomeSearchContentProps,
    'prefetchMovieData'
>
