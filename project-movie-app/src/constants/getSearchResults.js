import { fetchSearchResults } from "./fetchSearchResults"

export const getSearchResult = async (searchTerm) => {

    const movies = await fetchSearchResults(searchTerm)

    return movies.map((movie) => (
        {
            id: movie.id,
            title: movie.title,
            posterPath: movie.poster_path,
        }
    ))
}