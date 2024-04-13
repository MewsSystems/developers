import { useQuery, keepPreviousData } from '@tanstack/react-query'
import { getMovieSearchList } from './services'
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
