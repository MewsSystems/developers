import { Movie, MoviesPage } from "../types";

export type MoviesResponse = Readonly<{
    page: number;
    results: MovieResponse[];
    total_pages: number;
    total_results: number;
}>;

export type MovieResponse = Readonly<{
    id: number;
    title: string;
    original_language: string;
    original_title: string;
    poster_path: string | null;
    overview: string;
    release_date: string;
    vote_average: number;
    vote_count: number;
}>;

// TODO: own typeguard lib or smth like zod
export const movieResponseTypeguard = (value: unknown): value is MovieResponse => {
    return value !== null
        && typeof value === 'object'
        && 'id' in value
        && 'title' in value
        && 'original_language' in value
        && 'original_title' in value
        && 'overview' in value
        && 'poster_path' in value
        && 'release_date' in value
        && 'vote_average' in value
        && 'vote_count' in value
        && typeof value.id === 'number'
        && typeof value.original_language === 'string'
        && typeof value.original_title === 'string'
        && typeof value.overview === 'string'
        && (typeof value.poster_path === 'string' || value.poster_path === null)
        && typeof value.release_date === 'string'
        && typeof value.title === 'string'
        && typeof value.vote_average === 'number'
        && typeof value.vote_count === 'number';
}

export const moviesResponseTypeguard = (value: unknown): value is MoviesResponse => {
    return value !== null
        && typeof value === 'object'
        && 'page' in value
        && 'results' in value
        && 'total_pages' in value
        && 'total_results' in value
        && typeof value.page === 'number'
        && Array.isArray(value.results)
        && value.results.every(movieResponseTypeguard)
        && typeof value.total_pages === 'number'
        && typeof value.total_results === 'number';
}

export function moviePageFromMoviesResponse({
    page,
    results,
    total_pages,
    total_results,
}: MoviesResponse): MoviesPage {
    return {
        page,
        movies: results.map(movieFromMovieResponse),
        totalPages: total_pages,
        totalResults: total_results,
    };
}

export function movieFromMovieResponse({
    id,
    title,
    original_language,
    original_title,
    poster_path,
    overview,
    release_date,
    vote_average,
    vote_count,
}: MovieResponse): Movie {
    return {
        id,
        title,
        originalLanguage: original_language,
        originalTitle: original_title,
        posterPath: poster_path !== null ? poster_path : undefined,
        overview: overview,
        releaseDate: new Date(release_date),
        voteAverage: vote_average,
        voteCount: vote_count,
    };
}