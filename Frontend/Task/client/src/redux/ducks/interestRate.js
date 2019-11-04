import { ofType } from 'redux-observable';
import { ajax } from 'rxjs/ajax';
import { path } from 'ramda';
import { of } from 'rxjs';
import { map, switchMap, catchError } from 'rxjs/operators';
import { combineReducers } from 'redux';

export const FETCH_INTEREST_RATE_REQUEST = 'FETCH_INTEREST_RATE_REQUEST';
export const FETCH_INTEREST_RATE_SUCCESS = 'FETCH_INTEREST_RATE_SUCCESS';
export const FETCH_INTEREST_RATE_FAILURE = 'FETCH_INTEREST_RATE_FAILURE';

// this action will trigger side effects in middleware redux-observable
export const fetchInterestRateRequest = () => ({
    type: FETCH_INTEREST_RATE_REQUEST,
});

export const fetchInterestRateSuccess = InterestRate => ({
    type: FETCH_INTEREST_RATE_SUCCESS,
    payload: InterestRate
});

export const fetchInterestRateFailure = error => ({
    type: FETCH_INTEREST_RATE_FAILURE,
    payload: error
});

export const fetchInterestRateEpic = (action$) => action$.pipe(
    ofType(FETCH_INTEREST_RATE_REQUEST),
    switchMap(() => {
        return ajax.getJSON('http://localhost:8080/api/interestRate')
            .pipe(
                map(response => {
                    const {interestRate} = response.payload;
                    return fetchInterestRateSuccess(interestRate);
                }),
                catchError(error => of(fetchInterestRateFailure(error)))
            );
    })
);

// reducers to be combined
export const isLoadingReducer = (state = false, action) => {
    if (action.type === FETCH_INTEREST_RATE_REQUEST) return true;
    if (action.type === FETCH_INTEREST_RATE_SUCCESS) return false;
    return state;
};

export const errorReducer = (state = null, action) => {
    if (action.type === FETCH_INTEREST_RATE_FAILURE) return action.payload;
    return state;
};

export const dataReducer = (state = null, action) => {
    if (action.type === FETCH_INTEREST_RATE_SUCCESS) return action.payload;
    return state;
};

// selectors
export const selectInterestRate = path(['interestRate']);

// reducer
export default combineReducers({
    isLoading: isLoadingReducer,
    error: errorReducer,
    data: dataReducer,
});


