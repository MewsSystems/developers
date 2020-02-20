import { Dispatch } from 'redux';
import api from '../../../services/api';
import { MovieDetail } from '../../../types';

export enum DETAIL_ACTIONS {
    REQUEST_MOVIE = 'REQUEST_MOVIE',
    RECEIVE_MOVIE = 'RECEIVE_MOVIE',
    MOVIE_ERROR = 'MOVIE_ERROR',
}

export function getMovie() {
    return {
        type: DETAIL_ACTIONS.REQUEST_MOVIE,
    } as const;
}

export function receiveMovie(movie: MovieDetail) {
    return {
        type: DETAIL_ACTIONS.RECEIVE_MOVIE,
        movie,
    } as const;
}

export function getMovieFailed(error: any) {
    return {
        type: DETAIL_ACTIONS.MOVIE_ERROR,
        error,
    } as const;
}

export type DetailActions = ReturnType<typeof getMovie> | ReturnType<typeof receiveMovie> | ReturnType<typeof getMovieFailed>;

export function getMovieAction(id: number) {
    return async (dispatch: Dispatch) => {
        dispatch(getMovie());
        try {
            const movie = await api.getMovie(id);
            dispatch(receiveMovie({
                ...movie,
                poster_path: movie.poster_path ? `https://image.tmdb.org/t/p/w500/${movie.poster_path}` : null,
            }));
        } catch (exception) {
            dispatch(getMovieFailed(exception));
        }
    };
}
