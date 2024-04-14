import { useQuery, keepPreviousData } from '@tanstack/react-query'
import { getMovieDetail, getMovieSearchList } from './services'
import { MovieSearchProps } from './types'

export const useMovieSearchQuery = ({
    movieTitle,
    page = 1,
}: MovieSearchProps) =>
    useQuery({
        queryKey: ['search', movieTitle, page],
        queryFn: () => getMovieSearchList({ movieTitle, page }),
        placeholderData: keepPreviousData,
        enabled: movieTitle.length > 2,
    })

export const useMovieDetailQuery = (movie_id?: number) =>
    useQuery({
        queryKey: ['movie', movie_id],
        queryFn: () => getMovieDetail(movie_id),
        enabled: !!movie_id,
    })
