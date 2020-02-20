import {
    createStore, applyMiddleware, Store, combineReducers,
} from 'redux';
import { useSelector } from 'react-redux';
import thunk from 'redux-thunk';
import movieSearchReducer, { MovieSearchState } from './modules/movie-search/store/search-movie-reducer';
import movieDetailReducer, { MovieDetailState } from './modules/movie-detail/store/detail-movie-reducer';

export interface AppState {
    search: MovieSearchState,
    detail: MovieDetailState,
}

const store: Store<AppState, any> = createStore(combineReducers({
    search: movieSearchReducer,
    detail: movieDetailReducer,
}), applyMiddleware(thunk));

export function useAppSelector<Selected>(
    selector: (state: AppState) => Selected,
    equalityFn?: (left: Selected, right: Selected) => boolean,
) {
    return useSelector<AppState, Selected>(selector, equalityFn);
}

export default store;
