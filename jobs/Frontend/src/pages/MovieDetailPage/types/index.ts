import type { MovieDetail } from '../../../types'

export interface MovieDetailPageProps {
    detailData?: MovieDetail
    isLoading: boolean
    isError: boolean
    navigateBack: () => void
    navigateHome: () => void
    isPreviousPageAvailable: boolean
}

export interface UseMovieDetailHookProps {}
