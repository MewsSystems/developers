import { fetchPaginatedMovies } from "./fetchPaginatedMovies"
import { MovieCard, MovieApiItem } from "../types/movie"

export const getPopularMovies = async (page: number): Promise<MovieCard[]> => {

    const movies: MovieApiItem[]  = await fetchPaginatedMovies(page)

    return movies.map((movie) => (
        {
            id: movie.id,
            title: movie.title,
            posterPath: movie.poster_path,
        }
    ))
}