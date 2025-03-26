import { BASE_API_URL } from "./API.constants"
import { API_KEY } from "./API.constants"

export const fetchSearchResults = async (searchTerm) => {
    
    try {
        const reqMovies = await fetch(`${BASE_API_URL}/search/movie?api_key=${API_KEY}&query=${encodeURIComponent(searchTerm)}`)
        const movies = await reqMovies.json()

        return movies.results
    }
    catch (error) {
        console.error("Error fetching search results:", error)
        return []
    }
}