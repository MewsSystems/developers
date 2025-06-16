import { BASE_API_URL } from "./API.constants"
import { API_KEY } from "./API.constants"
import { MovieApiItem, MovieApiResponse } from "../types/movie"

export const fetchSearchResults = async (searchTerm: string, page: number):Promise<MovieApiItem[]> => {

    try {
        const reqMovies = await fetch(`${BASE_API_URL}/search/movie?api_key=${API_KEY}&query=${encodeURIComponent(searchTerm)}&page=${page}`)
        const movies: MovieApiResponse = await reqMovies.json()

        return movies.results
    }
    catch (error) {
        console.error("Error fetching search results:", error)
        return []
    }
}