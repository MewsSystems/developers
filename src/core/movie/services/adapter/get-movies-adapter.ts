import { Movie } from "../types/movie";

export const getMoviesAdapter = (movies: any[]): Movie[] => {
    return movies.map((movie) => ({
        id: movie.id ?? null,
        title: movie.original_title ?? null,
        overview: movie.overview ?? null,
        popularity: movie.popularity ?? null,
        posterPath: movie.poster_path ?? null,
        releaseDate: movie.release_date ?? null,
        video: movie.video ?? false,
        voteAverage: movie.vote_average ?? null,
        voteCount: movie.vote_count ?? null,
        backdropPath: movie.backdrop_path ?? null,
        runtime: movie.runtime ?? null,
        language: movie.original_language ?? null,
    }));
};