import { isObject } from '~data/types';

export const locationWithMovieStateTypeguard = (value: unknown): value is Readonly<{
    movie: string;
}> => {
    return isObject(value)
        && typeof value['movie'] === 'string';
}