import { fetchSearchResults } from "./fetchSearchResults"
import { MovieCard, MovieApiItem } from "../types/movie"

export const getSearchedMovies = async (searchTerm: string, page: number): Promise<MovieCard[]> => {

    const movies: MovieApiItem[] = await fetchSearchResults(searchTerm, page)

    return movies.map((movie) => (
        {
            id: movie.id,
            title: movie.title,
            posterPath: movie.poster_path,
        }
    ))
}