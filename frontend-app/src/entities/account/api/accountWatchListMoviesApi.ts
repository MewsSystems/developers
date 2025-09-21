import { baseGetApi } from "@/shared/api/baseApi";
import type { PageList } from "@/shared/api/types";

export async function getAccountWatchListMoviesApi({
  session_id,
  accountId,
}: {
  session_id: string;
  accountId: number;
}): Promise<PageList<any> | undefined> {
  return baseGetApi({
    version: "3",
    path: `account/${accountId}/watchlist/movies`,
    params: {
      session_id,
    },
  });
}
