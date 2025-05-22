import { Movie } from "../types/movie";

export const getMoviesAdapter = (movies: any[]): Movie[] => {
    return movies.map((movie) => ({
        id: movie.id,
        title: movie.title,
        overview: movie.overview,
        popularity: movie.popularity,
        poster_path: movie.poster_path,
        release_date: movie.release_date,
        video: movie.video,
        language: movie.language,
    }));
};