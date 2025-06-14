import { fetchMovieID } from "./fetchMovieID"
import { MovieDetailInfo, MovieDetailApiItem } from "../types/movieDetail"

export const getFormattedMovieID = async (id: number): Promise<MovieDetailInfo> => {

    const movie: MovieDetailApiItem = await fetchMovieID(id)

    return {
        title: movie.title?.trim() || "Title not available",
        tagline: movie.tagline || undefined,
        backdropPath: movie.backdrop_path
            ? `https://image.tmdb.org/t/p/w500${movie.backdrop_path}`
            : undefined,
        genres: movie.genres?.length
            ? movie.genres.map((g) => g.name).join(", ")
            : undefined,
        userScore:
            typeof movie.vote_average === "number" && movie.vote_average > 0
              ? `${Math.round(movie.vote_average * 10)}%`
              : undefined,
        releaseDate: movie.release_date
            ? new Date(movie.release_date).toLocaleDateString("cs-CZ")
            : undefined,
        originalLanguage: movie.original_language?.toUpperCase(),
        runtime:
            typeof movie.runtime === "number" && movie.runtime > 0
            ? `${movie.runtime} min`
            : undefined,
        homepage: movie.homepage || undefined,
        overview: movie.overview || undefined
    }
}