export const locationWithMovieStateTypeguard = (value: unknown): value is Readonly<{
    movie: string;
}> => {
    return value !== null
        && typeof value === 'object'
        && 'movie' in value
        && typeof value.movie === 'string'
}