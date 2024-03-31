export type MoviesPage = Readonly<{
    page: number;
    movies: readonly Movie[];
    totalPages: number;
    totalResults: number;
}>;

export type Movie = Readonly<{
    id: number;
    title: string;
    originalLanguage?: string;
    originalTitle?: string;
    posterPath?: string;
    overview?: string;
    releaseDate?: Date;
    voteAverage?: number;
    voteCount?: number;
}>;

// JSON.parse() will use strings for Date objects
export const movieLikeTypeguard = (value: unknown): value is Movie & Readonly<{ releaseDate: string }> => {
    return isObject(value)
        && isNumber(value['id'])
        && isString(value['title'])
        && isOptionalString(value['originalLanguage'])
        && isOptionalString(value['originalTitle'])
        && isOptionalString(value['posterPath'])
        && isOptionalString(value['overview'])
        && isOptionalString(value['releaseDate'])
        && isOptionalNumber(value['voteAverage'])
        && isOptionalNumber(value['voteCount']);
}

export function isSomething<T>(value: unknown): value is NonNullable<T> {
    return value !== null && value !== undefined;
}

export function isObject(value: unknown): value is Record<string, unknown> {
    return isSomething(value) && typeof value === 'object';
}

export function isString(value: unknown): value is string {
    return typeof value === 'string';
}

export function isOptionalString(value: unknown): value is string | undefined {
    return isString(value) || value === undefined;
}

export function isOptionalStringOrNull(value: unknown): value is string | undefined | null {
    return isString(value) || value === undefined || value === null;
}

export function isNumber(value: unknown): value is number {
    return typeof value === 'number';
}

export function isOptionalNumber(value: unknown): value is number | undefined {
    return isNumber(value) || value === undefined;
}

export function isOptionalNumberOrNull(value: unknown): value is number | undefined | null {
    return isNumber(value) || value === undefined || value === null;
}