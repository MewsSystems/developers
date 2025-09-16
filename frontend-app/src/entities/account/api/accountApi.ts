import { getApiKey, getBaseUrlApi } from "../../../shared/api/baseApi";
type AccoundResponse = {
    name: string;
    username: string;
    id: string;
}

export async function getAccountApi({ session_id }: { session_id: string }) {
    const TMDB_API_URL = getBaseUrlApi({ version: "3" });
    const API_KEY = getApiKey();
    const accountResponse = await fetch(
        `${TMDB_API_URL}/account?api_key=${API_KEY}&session_id=${session_id}`
    );
    return await accountResponse.json() as AccoundResponse;
}
