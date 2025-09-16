import { getApiKey, getBaseUrlApi } from "@/shared/api/baseApi";

type RequestTokenResponse = {
    success: boolean;
    request_token: string;
}

export async function requestTokenApi(): Promise<RequestTokenResponse> {
    const TMDB_API_URL = getBaseUrlApi({ version: "3" });
    const API_KEY = getApiKey();
    const response = await fetch(
        `${TMDB_API_URL}/authentication/token/new?api_key=${API_KEY}`
    );
    return await response.json() as RequestTokenResponse;
}