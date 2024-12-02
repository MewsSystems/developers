import { useLocation, useNavigate, useParams } from 'react-router-dom'
import {
    useMovieDetailQuery,
    useMovieSimilarQuery,
} from '../../../api/movies/hooks'
import type { MovieDetailPageProps, UseMovieDetailHookProps } from '../types'
import { useQueryClient } from '@tanstack/react-query'
import { getSimilarMovies } from '../../../api/movies/services'

export const usePage = (
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    props: UseMovieDetailHookProps,
): MovieDetailPageProps => {
    const { id: movie_id } = useParams()
    const location = useLocation()
    const navigate = useNavigate()
    const queryClient = useQueryClient()
    const {
        data: detailData,
        isLoading,
        isError,
    } = useMovieDetailQuery(Number(movie_id))

    const {
        data: similarMovieData,
        isLoading: isSimilarLoading,
        isError: isSimilarError,
    } = useMovieSimilarQuery(Number(movie_id))

    const isPreviousPageAvailable = location.key !== 'default'

    const navigateBack = () => {
        isPreviousPageAvailable ? navigate(-1) : navigate('/')
    }

    const navigateHome = () => {
        navigate('/')
    }

    const prefetchSimilarMovieData = async (similarMovieId: number) => {
        await queryClient.prefetchQuery({
            queryKey: ['search', 'similar-item', similarMovieId],
            queryFn: () => getSimilarMovies(similarMovieId),
        })
    }

    return {
        detailData,
        isLoading,
        isError,
        similarMovieData,
        isSimilarLoading,
        isSimilarError,
        prefetchSimilarMovieData,
        navigateBack,
        navigateHome,
        isPreviousPageAvailable,
    }
}
