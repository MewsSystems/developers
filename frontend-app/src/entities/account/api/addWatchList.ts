import { basePostApi } from "@/shared/api/baseApi";

export async function addWatchList({
  movieId,
  watchlist,
  accountId,
  sessionId,
}: {
  movieId: number;
  watchlist: boolean;
  accountId: number;
  sessionId: string;
}) {
  const payload = {
    media_type: "movie",
    media_id: movieId,
    watchlist: watchlist,
  };

  return basePostApi({
    version: "3",
    payload,
    path: `account/${accountId}/watchlist`,
    params: { session_id: sessionId },
  });
}
