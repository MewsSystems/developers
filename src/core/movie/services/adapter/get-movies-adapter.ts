import { Movie } from "../types/movie";

export const getMoviesAdapter = (movies: any[]): Movie[] => {
    return movies.map((movie) => ({
        id: movie.id,
        title: movie.original_title,
        overview: movie.overview,
        popularity: movie.popularity,
        posterPath: movie.poster_path,
        releaseDate: movie.release_date,
        video: movie.video,
        voteAverage: movie.vote_average,
        voteCount: movie.vote_count,
        backdropPath: movie.backdrop_path,
        runtime: movie.runtime,
        genreIds: movie.genre_ids,
        language: movie.original_language,
    }));
};