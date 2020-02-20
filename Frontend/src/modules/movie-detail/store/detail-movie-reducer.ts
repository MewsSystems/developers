// eslint-disable-next-line filenames/match-exported
import { DETAIL_ACTIONS, DetailActions } from './detail-movie-actions';
import { MovieDetail } from '../../../types';

export interface MovieDetailState {
    movie?: MovieDetail,
    isFetching: boolean,
    error: any,
}

const initialState: MovieDetailState = {
    movie: undefined,
    isFetching: true,
    error: undefined,
};

export default function movieDetailReducer(state = initialState, action: DetailActions) {
    if (action.type === DETAIL_ACTIONS.REQUEST_MOVIE) {
        return {
            isFetching: true,
        };
    }
    if (action.type === DETAIL_ACTIONS.RECEIVE_MOVIE) {
        return {
            isFetching: false,
            movie: action.movie,
        };
    }
    if (action.type === DETAIL_ACTIONS.MOVIE_ERROR) {
        return {
            isFetching: false,
            error: action.error,
        };
    }
    return state;
}
