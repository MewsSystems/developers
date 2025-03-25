import { BASE_API_URL } from "./API.constants"
import { API_KEY } from "./API.constants"

export const fetchMovieID = async (id) => {

    try {
        const reqMovie = await fetch(`${BASE_API_URL}/movie/${id}?api_key=${API_KEY}&language=en-US`)
        const movie = await reqMovie.json()

        return movie
    }
    catch (error) {
        console.error("Error fetching movie ID:", error)
        return []
    }
}