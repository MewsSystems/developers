import {
    useQuery,
} from '@tanstack/react-query'
import { getMovies } from '@/entities/movie/api/getMoviesApi'
import { usePreferredLanguage } from '@uidotdev/usehooks';

export default function useQueryMovies({ query, page }: { query: string, page: string }) {
    const language = usePreferredLanguage();
    return useQuery({ queryKey: ['movies', query, page], queryFn: () => getMovies({ query, page, language }) })
}
