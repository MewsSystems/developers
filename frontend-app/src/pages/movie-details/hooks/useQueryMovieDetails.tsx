import { useQuery } from "@tanstack/react-query";
import { usePreferredLanguage } from "@uidotdev/usehooks";
import { getDetails } from "@/pages/movie-details/api/getDetails";
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";

export function useQueryMovieDetails({ movie_id }: { movie_id: string }) {
  const language = usePreferredLanguage();
  const auth = useAuth();
  return useQuery({
    queryKey: ["moviedetails", movie_id],
    queryFn: () =>
      getDetails({ movie_id }, { language, session_id: auth.sessionId+"" }),
  });
}
