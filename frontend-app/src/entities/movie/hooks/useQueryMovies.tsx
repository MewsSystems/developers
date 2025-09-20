import {
    useQuery,
} from '@tanstack/react-query'
import { getMovies } from '@/entities/movie/api/getMoviesApi'

export default function useQueryMovies({ query, page, language }: { query: string, page: string, language: string }) {
    return useQuery({ queryKey: ['movies', query, page], queryFn: () => getMovies({ query, page, language }) })
}
