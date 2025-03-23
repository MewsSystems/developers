import { BASE_API_URL, API_KEY } from "../constants/API.constants"

export const fetchPaginatedMovies = async (page) => {

    try {
        const reqMovies = await fetch(`${BASE_API_URL}/movie/popular?api_key=${API_KEY}&language=en-US&page=${page}`)
        const movies = await reqMovies.json()

        return movies.results
    }
    catch (error) {
        console.error("Error fetching popular movies:", error)
        return []
    }
}