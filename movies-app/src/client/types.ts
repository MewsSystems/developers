export type MoviesResponse = Readonly<{
    page: number;
    results: Movie[];
    total_pages: number;
    total_results: number;
}>;

export type Movie = Readonly<{
    adult: boolean;
    backdrop_path: string;
    genre_ids: number[];
    id: number;
    original_language: string;
    original_title: string;
    overview: string;
    popularity: number;
    poster_path: string;
    release_date: string;
    title: string;
    video: boolean;
    vote_average: number;
    vote_count: number;
}>;

// TODO: own typeguard lib or zod
export const movieTypeguard = (value: unknown): value is Movie => {
    return value !== null
        && typeof value === 'object'
        && 'adult' in value
        && 'backdrop_path' in value
        && 'genre_ids' in value
        && 'id' in value
        && 'original_language' in value
        && 'original_title' in value
        && 'overview' in value
        && 'popularity' in value
        && 'poster_path' in value
        && 'release_date' in value
        && 'title' in value
        && 'video' in value
        && 'vote_average' in value
        && 'vote_count' in value
        && typeof value.adult === 'boolean'
        && typeof value.backdrop_path === 'string'
        && Array.isArray(value.genre_ids)
        && value.genre_ids.every((element: unknown) => typeof element === 'number')
        && typeof value.id === 'number'
        && typeof value.original_language === 'string'
        && typeof value.original_title === 'string'
        && typeof value.overview === 'string'
        && typeof value.popularity === 'number'
        && typeof value.poster_path === 'string'
        && typeof value.release_date === 'string'
        && typeof value.title === 'string'
        && typeof value.video === 'boolean'
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
        && value.results.every(movieTypeguard)
        && typeof value.total_pages === 'number'
        && typeof value.total_results === 'number';
}