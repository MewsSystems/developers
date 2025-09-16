import { getApiKey, getBaseUrlApi } from "@/shared/api/baseApi";

type SessionResponse = {
    success: boolean;
    session_id: string;
}

export async function createSessionApi(requestToken: string): Promise<SessionResponse> {
    const TMDB_API_URL = getBaseUrlApi({ version: "3" });
    const API_KEY = getApiKey();
    const response = await fetch(

        `${TMDB_API_URL}/authentication/session/new?api_key=${API_KEY}`,
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ request_token: requestToken }),
        }
    );
    return await response.json() as SessionResponse;
}