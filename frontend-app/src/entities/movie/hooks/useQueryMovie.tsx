import {
    useQuery,
} from '@tanstack/react-query'
import { getMovie } from '@/entities/movie/api/getMovieApi'
import { usePreferredLanguage } from '@uidotdev/usehooks';
import { useAuth } from '@/entities/auth/api/providers/AuthProvider';

export default function useQueryMovie({ movie_id }: { movie_id: string }) {
    const language = usePreferredLanguage();
    const auth = useAuth();
    return useQuery({ queryKey: ['movie', movie_id], queryFn: () => getMovie({ movie_id }, { language, session_id: auth.sessionId+"" }) })
}
