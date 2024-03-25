import {API_KEY, API_URL} from "@/const";
import {APIResponse, Movie} from "@/types";

export const searchMovies = async (query: string, page: number): Promise<APIResponse> => {
    const data = await fetch(`${API_URL}/search/movie?query=${query}&api_key=${API_KEY}&page=${page}`)
    return data.json()
}

export const getMovieDetails = async (id: number): Promise<Movie> => {
    const data = await fetch(`${API_URL}/movie/${id}?api_key=${API_KEY}`)
    return data.json()
}
