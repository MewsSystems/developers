import type { MovieSearchCollection } from '../../../api/movies/types'
import type { MovieDetail } from '../../../types'

export * from './components'

export interface MovieDetailPageProps {
    detailData?: MovieDetail
    isLoading: boolean
    isError: boolean
    navigateBack: () => void
    navigateHome: () => void
    isPreviousPageAvailable: boolean
    similarMovieData?: MovieSearchCollection
    isSimilarLoading: boolean
    isSimilarError: boolean
    prefetchSimilarMovieData: (similarMovieId: number) => Promise<void>
}

export interface UseMovieDetailHookProps {}
