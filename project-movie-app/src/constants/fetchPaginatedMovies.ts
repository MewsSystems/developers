import { BASE_API_URL, API_KEY } from "./API.constants"
import { MovieApiItem, MovieApiResponse } from "../types/movie"

export const fetchPaginatedMovies = async (page: number): Promise<MovieApiItem[]> => {

    try {
        const reqMovies = await fetch(`${BASE_API_URL}/movie/popular?api_key=${API_KEY}&language=en-US&page=${page}`)
        const movies: MovieApiResponse = await reqMovies.json()

        return movies.results
    }
    catch (error) {
        console.error("Error fetching popular movies:", error)
        return []
    }
}