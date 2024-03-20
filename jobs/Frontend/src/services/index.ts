import {API_KEY, API_URL} from "@/const";
import {API_Response, Movie} from "@/types";

export const searchMovies = async (query: string): Promise<API_Response> => {
    const data = await fetch(`${API_URL}/search/movie?query=${query}&api_key=${API_KEY}`)
    return data.json()
}

export const getMovieDetails = async (id: number): Promise<Movie> => {
    const data = await fetch(`${API_URL}/movie/${id}?api_key=${API_KEY}`)
    return data.json()
}
