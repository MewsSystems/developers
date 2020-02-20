// eslint-disable-next-line filenames/match-exported
import { SEARCH_ACTIONS, SearchActions } from './search-movie-actions';
import { Movie } from '../../../types';

export interface MovieSearchState {
    page: number,
    movies: Movie[],
    results: number,
    isFetching: boolean,
    totalPages: number,
    error: any,
    query: string,
}

const initialState: MovieSearchState = {
    query: '',
    page: 1,
    movies: [],
    results: 0,
    totalPages: 0,
    isFetching: false,
    error: undefined,
};

export default function searchMovieReducer(state = initialState, action: SearchActions) {
    if (action.type === SEARCH_ACTIONS.CHANGE_QUERY) {
        return {
            ...initialState,
            query: action.query,
        };
    }
    if (action.type === SEARCH_ACTIONS.CHANGE_PAGE) {
        return {
            ...state,
            page: action.page,
        };
    }
    if (action.type === SEARCH_ACTIONS.REQUEST_MOVIES) {
        return {
            ...state,
            isFetching: true,
            error: undefined,
        };
    }
    if (action.type === SEARCH_ACTIONS.RECEIVE_MOVIES) {
        return {
            ...state,
            isFetching: false,
            page: action.response.page,
            results: action.response.total_results,
            movies: action.response.results,
            totalPages: action.response.total_pages,
        };
    }
    if (action.type === SEARCH_ACTIONS.MOVIES_ERROR) {
        return {
            ...state,
            isFetching: false,
            error: action.error,
        };
    }
    return state;
}
