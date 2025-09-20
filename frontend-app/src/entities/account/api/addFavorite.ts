import { basePostApi } from "@/shared/api/baseApi";

export async function addFavorite({
  movieId,
  favorite,
  accountId,
  sessionId,
}: {
  movieId: number;
  favorite: boolean;
  accountId: number;
  sessionId: string;
}) {
  const payload = {
    media_type: "movie",
    media_id: movieId,
    favorite: favorite,
  };

  return basePostApi({
    version: "3",
    payload,
    path: `account/${accountId}/favorite`,
    params: { session_id: sessionId },
  });
}
