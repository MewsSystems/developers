import { BASE_API_URL, API_KEY } from "./API.constants"
import { MovieDetailApiItem } from "../types/movieDetail"

export const fetchMovieID = async (id: number): Promise<MovieDetailApiItem> => {

    try {
        const reqMovie = await fetch(`${BASE_API_URL}/movie/${id}?api_key=${API_KEY}&language=en-US`)
        const movie: MovieDetailApiItem = await reqMovie.json()

        return movie
    }
    catch (error) {
        console.error("Error fetching movie ID:", error)
        throw error
    }
}