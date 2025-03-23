import { fetchPaginatedMovies } from "./fetchPaginatedMovies"

export const getFormattedMovies = async (page) => {

    const movies = await fetchPaginatedMovies(page)

    return movies.map((movie) => (
        {
            id: movie.id,
            title: movie.original_title,
            posterPath: movie.poster_path,
        }
    ))
}