import { fetchMovieID } from "./fetchMovieID"

export const getFormattedMovieID = async (id) => {

    const movie = await fetchMovieID(id)

    return {
        title: movie.title ? movie.title : "Title not available",
        tagline: movie.tagline && movie.tagline,
        backdropPath: movie.backdrop_path && `https://image.tmdb.org/t/p/w500${movie.backdrop_path}`,
        genres: movie.genres.length > 0 && movie.genres.map(g => g.name).join(", "),
        userScore: Number(movie.vote_average) > 0 && `${Math.round(movie.vote_average * 10)}%`,
        releaseDate: movie.release_date && new Date(movie.release_date).toLocaleDateString("cs-CZ"),
        originalLanguage: movie.originalLanguage && movie.original_language.toUpperCase(),
        runtime: Number(movie.runtime) > 0 && `${movie.runtime} min`,
        homepage: movie.homepage && movie.homepage,
        overview: movie.overview && movie.overview
    }
}