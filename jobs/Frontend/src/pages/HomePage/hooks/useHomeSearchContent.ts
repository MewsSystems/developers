import { useQueryClient } from '@tanstack/react-query'
import { HomeSearchContentProps, UseHomeSearchHook } from '../types'
import { getMovieDetail } from '../../../api'

export const useHomeSearchContent = (
    props: UseHomeSearchHook,
): HomeSearchContentProps => {
    const queryClient = useQueryClient()

    const prefetchMovieData = async (movie_id: number) => {
        await queryClient.prefetchQuery({
            queryKey: ['movie', movie_id],
            queryFn: () => getMovieDetail(movie_id),
        })
    }

    return {
        ...props,
        prefetchMovieData,
    }
}
