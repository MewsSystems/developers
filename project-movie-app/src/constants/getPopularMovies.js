import { fetchPaginatedMovies } from "./fetchPaginatedMovies"

export const getPopularMovies = async (page) => {

    const movies = await fetchPaginatedMovies(page)

    return movies.map((movie) => (
        {
            id: movie.id,
            title: movie.title,
            posterPath: movie.poster_path,
        }
    ))
}