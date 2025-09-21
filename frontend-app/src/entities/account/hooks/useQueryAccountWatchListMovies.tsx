import { useQuery } from "@tanstack/react-query";
import { useAuth } from "@/entities/auth/api/providers/AuthProvider";
import { getAccountWatchListMoviesApi } from "../api/accountWatchListMoviesApi";

export default function useQueryAccountWatchListMovies({
  accountId,
}: {
  accountId: number;
}) {
  const auth = useAuth();
  return useQuery({
    queryKey: ["account/watchlist/movies", accountId],
    queryFn: () =>
      getAccountWatchListMoviesApi({
        accountId,
        session_id: auth.sessionId ?? "",
      }),
  });
}
