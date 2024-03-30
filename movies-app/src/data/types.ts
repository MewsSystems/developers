export type MoviesPage = Readonly<{
    page: number;
    movies: readonly Movie[];
    totalPages: number;
    totalResults: number;
}>;

export type Movie = Readonly<{
    id: number;
    title: string;
    originalLanguage: string;
    originalTitle: string;
    posterPath?: string;
    overview?: string;
    releaseDate: Date;
    voteAverage: number;
    voteCount: number;
}>;

// JSON.parse() will use strings for Date objects
export const movieLikeTypeguard = (value: unknown): value is Movie & Readonly<{ releaseDate: string }> => {
    return value !== null
        && typeof value === 'object'
        && 'id' in value
        && 'title' in value
        && 'originalLanguage' in value
        && 'originalTitle' in value
        && 'overview' in value
        && 'posterPath' in value
        && 'releaseDate' in value
        && 'voteAverage' in value
        && 'voteCount' in value
        && typeof value.id === 'number'
        && typeof value.originalLanguage === 'string'
        && typeof value.originalTitle === 'string'
        && (typeof value.overview === 'string' || value.overview === undefined)
        && (typeof value.posterPath === 'string' || value.posterPath === undefined)
        && typeof value.releaseDate === 'string'
        && typeof value.title === 'string'
        && typeof value.voteAverage === 'number'
        && typeof value.voteCount === 'number';
}

export function isSomething<T>(value: unknown): value is NonNullable<T> {
    return value !== null && value !== undefined;
}