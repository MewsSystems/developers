import { useLocation, useNavigate, useParams } from 'react-router-dom'
import { useMovieDetailQuery } from '../../../api/movies/hooks'
import { MovieDetailPageProps, UseMovieDetailHookProps } from '../types'

export const usePage = (
    props: UseMovieDetailHookProps,
): MovieDetailPageProps => {
    const { id: movie_id } = useParams()
    const location = useLocation()
    const navigate = useNavigate()
    const {
        data: detailData,
        isLoading,
        isError,
    } = useMovieDetailQuery(Number(movie_id))

    const isPreviousPageAvailable = location.key !== 'default'

    const navigateBack = () => {
        isPreviousPageAvailable ? navigate(-1) : navigate('/')
    }

    const navigateHome = () => {
        navigate('/')
    }

    return {
        detailData,
        isLoading,
        isError,
        navigateBack,
        navigateHome,
        isPreviousPageAvailable,
    }
}
