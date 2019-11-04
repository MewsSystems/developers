import { ofType } from 'redux-observable';
import { ajax } from 'rxjs/ajax';
import { path, isEmpty } from 'ramda';
import { of } from 'rxjs';
import { map, switchMap, catchError, debounceTime } from 'rxjs/operators';
import { combineReducers } from 'redux';

export const FETCH_CURRENCY_PAIRS_RATES_REQUEST = 'FETCH_CURRENCY_PAIRS_RATES_REQUEST';
export const FETCH_CURRENCY_PAIRS_RATES_SUCCESS = 'FETCH_CURRENCY_PAIRS_RATES_SUCCESS';
export const FETCH_CURRENCY_PAIRS_RATES_FAILURE = 'FETCH_CURRENCY_PAIRS_RATES_FAILURE';

// this action will trigger side effects in middleware redux-observable
export const fetchCurrencyPairsRates = (currencyPairsIds = []) => ({
    type: FETCH_CURRENCY_PAIRS_RATES_REQUEST,
    payload: currencyPairsIds,
});

export const fetchCurrencyPairsRatesSuccess = (currencyPairsRates = {}) => ({
    type: FETCH_CURRENCY_PAIRS_RATES_SUCCESS,
    payload: currencyPairsRates
});

export const fetchCurrencyPairsRatesFailure = (error = null) => ({
    type: FETCH_CURRENCY_PAIRS_RATES_FAILURE,
    payload: error
});

const getUrl = (currencyPairsIds = []) => {
    let url = '/rates';
    if (!isEmpty(currencyPairsIds)) {
        currencyPairsIds.forEach((id, index) => {
            if (index === 0) {
                url = `${url}?currencyPairIds[]=${id}`;
            } else {
                url = `${url}&&currencyPairIds[]=${id}`;
            }
        });
    }
    return url;
};

export const fetchCurrencyPairsRatesEpic = (action$) => action$.pipe(
    ofType(FETCH_CURRENCY_PAIRS_RATES_REQUEST),
    debounceTime(500),
    switchMap(action => {
        const currencyPairsIds = action.payload;
        return ajax.getJSON(getUrl(currencyPairsIds))
            .pipe(
                map(response => {
                    const {rates} = response;
                    return fetchCurrencyPairsRatesSuccess(rates);
                }),
                catchError(error => of(fetchCurrencyPairsRatesFailure(error)))
            );
    })
);

// reducers to be combined
export const loadingReducer = (state = false, action) => {
    if (action.type === FETCH_CURRENCY_PAIRS_RATES_REQUEST) return true;
    if (action.type === FETCH_CURRENCY_PAIRS_RATES_SUCCESS) return false;
    return state;
};

export const errorReducer = (state = null, action) => {
    if (action.type === FETCH_CURRENCY_PAIRS_RATES_FAILURE) return action.payload;
    if (action.type === FETCH_CURRENCY_PAIRS_RATES_SUCCESS) return null;
    if (action.type === FETCH_CURRENCY_PAIRS_RATES_REQUEST) return null;
    return state;
};

export const dataReducer = (state = {}, action) => {
    if (action.type === FETCH_CURRENCY_PAIRS_RATES_SUCCESS) return action.payload;
    return state;
};

// selectors
export const selectCurrencyPairsRates = path(['currencyPairsRates']);

// reducer
export default combineReducers({
    loading: loadingReducer,
    error: errorReducer,
    data: dataReducer,
});
