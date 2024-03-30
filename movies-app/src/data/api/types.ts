import {
    isNumber,
    isObject,
    isOptionalNumberOrNull,
    isOptionalStringOrNull,
    isSomething,
    isString,
    Movie,
    MoviesPage
} from "../types";

export type MoviesResponse = Readonly<{
    page: number;
    results: MovieResponse[];
    total_pages: number;
    total_results: number;
}>;

export type MovieResponse = Readonly<{
    id: number;
    title: string;
    original_language?: string | null;
    original_title?: string | null;
    poster_path?: string | null;
    overview?: string | null;
    release_date?: string | null;
    vote_average?: number | null;
    vote_count?: number | null;
}>;

// TODO: own typeguard lib or smth like zod
export const movieResponseTypeguard = (value: unknown): value is MovieResponse => {
    return isObject(value)
        && isString(value['title'])
        && isNumber(value['id'])
        && isOptionalStringOrNull(value['original_language'])
        && isOptionalStringOrNull(value['original_title'])
        && isOptionalStringOrNull(value['poster_path'])
        && isOptionalStringOrNull(value['overview'])
        && isOptionalStringOrNull(value['release_date'])
        && isOptionalNumberOrNull(value['vote_average'])
        && isOptionalNumberOrNull(value['vote_count']);
}

export const moviesResponseTypeguard = (value: unknown): value is MoviesResponse => {
    return isObject(value)
        && isNumber(value['page'])
        && Array.isArray(value['results'])
        && value.results.every(movieResponseTypeguard)
        && isNumber(value['total_pages'])
        && isNumber(value['total_results']);
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
        originalLanguage: original_language ?? undefined,
        originalTitle: original_title ?? undefined,
        posterPath: poster_path ?? undefined,
        overview: overview ?? undefined,
        releaseDate: release_date ? new Date(release_date) : undefined,
        // sometimes vote_average is 0, but it's not a valid value
        voteAverage: isSomething(vote_average) && vote_average !== 0 ? vote_average : undefined,
        voteCount: isSomething(vote_count) && vote_count !== 0 ? vote_count : undefined,
    };
}