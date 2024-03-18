import {API_KEY, API_URL} from "@/const";
import {API_Response} from "@/types";


export const searchMovies = async (query: string): Promise<API_Response> => {
    const data = await fetch(`${API_URL}?query=${query}&api_key=${API_KEY}`)
    return data.json()
}
