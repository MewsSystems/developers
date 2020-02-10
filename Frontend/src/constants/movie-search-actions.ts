import { MovieResult } from "../services/types";

export const MOVIE_SEARCH = 'MOVIE_SEARCH';
export const MOVIE_SEARCH_STARTED = 'MOVIE_SEARCH_STARTED';
export const MOVIE_SEARCH_SUCCESS = 'MOVIE_SEARCH_SUCCESS';
export const MOVIE_SEARCH_ERROR = 'MOVIE_SEARCH_ERROR';

export const PAGINATION_CHANGE_PAGE = 'PAGINATION_CHANGE_PAGE';


export type MovieSearchStartedAction = {
  type: typeof MOVIE_SEARCH_STARTED,
  payload: { isFetching: boolean, query: string, error?: string },
};

export type MovieSearchSuccessAction = {
  type: typeof MOVIE_SEARCH_SUCCESS,
  payload: { isFetching: boolean, results: MovieResult[] },
};

export type MovieSearchErrorAction = {
  type: typeof MOVIE_SEARCH_ERROR,
  payload: { error: string, isFetching: boolean }
}

export type PaginationChangePageAction = {
  type: typeof PAGINATION_CHANGE_PAGE,
  payload: { page: number }
}
