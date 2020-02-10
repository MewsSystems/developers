import { Movie } from "../services/types";

export const MOVIE_LOAD = 'MOVIE_LOAD';
export const MOVIE_LOAD_STARTED = 'MOVIE_LOAD_STARTED';
export const MOVIE_LOAD_SUCCESS = 'MOVIE_LOAD_SUCCESS';
export const MOVIE_LOAD_ERROR = 'MOVIE_LOAD_ERROR';

export type MovieLoadStartedAction = {
  type: typeof MOVIE_LOAD_STARTED,
  payload: { isFetching: boolean, error: string }
}

export type MovieLoadSuccessAction = {
  type: typeof MOVIE_LOAD_SUCCESS,
  payload: { isFetching: boolean, movie: Movie }
}

export type MovieLoadErrorAction = {
  type: typeof MOVIE_LOAD_ERROR,
  payload: { isFetching: boolean, error: string }
}
