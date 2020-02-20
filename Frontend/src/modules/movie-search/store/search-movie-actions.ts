import { Dispatch } from 'redux';
import api from '../../../services/api';
import { SearchResponse } from '../../../types';
import { AppState } from '../../../store';

export enum SEARCH_ACTIONS {
    CHANGE_QUERY = 'CHANGE_QUERY',
    CHANGE_PAGE = 'CHANGE_PAGE',
    REQUEST_MOVIES = 'REQUEST_MOVIES',
    RECEIVE_MOVIES = 'RECEIVE_MOVIES',
    MOVIES_ERROR = 'MOVIES_ERROR',
}

export function changeQuery(query: string) {
    return {
        type: SEARCH_ACTIONS.CHANGE_QUERY,
        query,
    } as const;
}

export function changePage(page: number) {
    return {
        type: SEARCH_ACTIONS.CHANGE_PAGE,
        page,
    } as const;
}

export function searchForMovies() {
    return {
        type: SEARCH_ACTIONS.REQUEST_MOVIES,
    } as const;
}

export function receiveMovies(response: SearchResponse) {
    return {
        type: SEARCH_ACTIONS.RECEIVE_MOVIES,
        response,
    } as const;
}

export function searchForMoviesFailed(error: any) {
    return {
        type: SEARCH_ACTIONS.MOVIES_ERROR,
        error,
    } as const;
}

export type SearchActions = ReturnType<typeof changeQuery> |
    ReturnType<typeof changePage> |
    ReturnType<typeof searchForMovies> |
    ReturnType<typeof receiveMovies> |
    ReturnType<typeof searchForMoviesFailed>;

export function searchMovieAction() {
    return async (dispatch: Dispatch, getState: () => AppState) => {
        dispatch(searchForMovies());
        const { query, page } = getState().search;
        try {
            const result = await api.search(query, page);
            dispatch(receiveMovies(result));
        } catch (exception) {
            dispatch(searchForMoviesFailed(exception));
        }
    };
}
