import { ofType } from 'redux-observable';
import { ajax } from 'rxjs/ajax';
import { path } from 'ramda';
import { of } from 'rxjs';
import { map, switchMap, catchError } from 'rxjs/operators';
import { combineReducers } from 'redux';

export const FETCH_CURRENCY_PAIRS_REQUEST = 'FETCH_CURRENCY_PAIRS_REQUEST';
export const FETCH_CURRENCY_PAIRS_SUCCESS = 'FETCH_CURRENCY_PAIRS_SUCCESS';
export const FETCH_CURRENCY_PAIRS_FAILURE = 'FETCH_CURRENCY_PAIRS_FAILURE';

// this action will trigger side effects in middleware redux-observable
export const fetchCurrencyPairs = () => ({
    type: FETCH_CURRENCY_PAIRS_REQUEST,
});

export const fetchCurrencyPairsSuccess = currencyPairs => ({
    type: FETCH_CURRENCY_PAIRS_SUCCESS,
    payload: currencyPairs
});

export const fetchCurrencyPairsFailure = error => ({
    type: FETCH_CURRENCY_PAIRS_FAILURE,
    payload: error
});

export const fetchCurrencyPairsEpic = (action$) => action$.pipe(
    ofType(FETCH_CURRENCY_PAIRS_REQUEST),
    switchMap(() => {
        return ajax.getJSON('/configuration')
            .pipe(
                map(response => {
                    const {currencyPairs} = response;
                    return fetchCurrencyPairsSuccess(currencyPairs);
                }),
                catchError(error => of(fetchCurrencyPairsFailure(error)))
            );
    })
);

// reducers to be combined
export const loadingReducer = (state = false, action) => {
    if (action.type === FETCH_CURRENCY_PAIRS_REQUEST) return true;
    if (action.type === FETCH_CURRENCY_PAIRS_SUCCESS) return false;
    return state;
};

export const errorReducer = (state = null, action) => {
    if (action.type === FETCH_CURRENCY_PAIRS_FAILURE) return action.payload;
    return state;
};

export const dataReducer = (state = {}, action) => {
    if (action.type === FETCH_CURRENCY_PAIRS_SUCCESS) return action.payload;
    return state;
};

// selectors
export const selectCurrencyPairs = path(['currencyPairs']);

// reducer
export default combineReducers({
    loading: loadingReducer,
    error: errorReducer,
    data: dataReducer,
});
