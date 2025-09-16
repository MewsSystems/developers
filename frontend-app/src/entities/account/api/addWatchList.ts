import { getApiKey, getBaseUrlApi } from "@/shared/api/baseApi";

export async function addWatchList({ movieId, watchlist, accountId, sessionId }: { movieId: number, watchlist: boolean, accountId: number, sessionId: string }) {
    const payload = {
        "media_type": "movie",
        "media_id": movieId,
        "watchlist": watchlist
    }

    const TMDB_API_URL = getBaseUrlApi({ version: "3" });
    const API_KEY = getApiKey();

    const response = await fetch(
        `${TMDB_API_URL}/account/${accountId}/watchlist?api_key=${API_KEY}&session_id=${sessionId}`,
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload),
        }

    );

    return await response.json() as any;
}
