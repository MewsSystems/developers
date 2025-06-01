import {BASE_URL} from "../const/base-url.ts";

export const fetchMovies = async ({query, pageParam = 1}: { query: string, pageParam: number }) => {
    const apiKey = import.meta.env.VITE_TMDB_API_KEY;
    const endpoint = query === "" ?
        `${BASE_URL}/movie/popular?api_key=${apiKey}&language=en-US&page=${pageParam}`
        : `${BASE_URL}/search/movie?api_key=${apiKey}&query=${query}&page=${pageParam}`

    const response = await fetch(endpoint)
    return response.json()
}
