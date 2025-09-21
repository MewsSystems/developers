import { useQuery } from "@tanstack/react-query";
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { getAccountFavoriteMoviesApi } from "../api/accountFavoriteMoviesApi";

export default function useQueryAccountFavoriteMovies({
  accountId,
}: {
  accountId: number;
}) {
  const auth = useAuth();
  return useQuery({
    queryKey: ["account/favorite/movies", accountId],
    queryFn: () =>
      getAccountFavoriteMoviesApi({
        accountId,
        session_id: auth.sessionId ?? "",
      }),
  });
}
