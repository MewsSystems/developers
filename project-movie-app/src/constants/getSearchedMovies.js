import { fetchSearchResults } from "./fetchSearchResults"

export const getSearchedMovies = async (searchTerm, page) => {

    const movies = await fetchSearchResults(searchTerm, page)

    return movies.map((movie) => (
        {
            id: movie.id,
            title: movie.title,
            posterPath: movie.poster_path,
        }
    ))
}