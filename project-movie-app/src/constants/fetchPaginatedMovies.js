import { BASE_API_URL, API_KEY } from "../constants/API.constants"

export const fetchPaginatedMovies = async ({page = 1}) => {

    try {
        const reqMovies = await fetch(`${BASE_API_URL}/movie/popular?api_key=${API_KEY}&page=${page}`)
        const movies = await reqMovies.json()

        return movies.results
    }
    catch (error) {
        console.error("Error fetching popular movies:", error)
        return []
    }
}