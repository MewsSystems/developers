import { ofType } from 'redux-observable';
import { ajax } from 'rxjs/ajax';
import {path} from 'ramda'
import { of } from 'rxjs';
import { debounceTime, map, switchMap, catchError } from 'rxjs/operators';
import { combineReducers } from 'redux';

export const FETCH_INSTALMENT_REQUEST = 'FETCH_INSTALMENT_REQUEST';
export const FETCH_INSTALMENT_SUCCESS = 'FETCH_INSTALMENT_SUCCESS';
export const FETCH_INSTALMENT_FAILURE = 'FETCH_INSTALMENT_FAILURE';

// this action will trigger side effects in middleware redux-observable
export const fetchInstalmentRequest = loanData => ({
    type: FETCH_INSTALMENT_REQUEST,
    payload: loanData
});

export const fetchInstalmentSuccess = instalment => ({
    type: FETCH_INSTALMENT_SUCCESS,
    payload: instalment
});

export const fetchInstalmentFailure = error => ({
    type: FETCH_INSTALMENT_FAILURE,
    payload: error
});

const url = ({amount, months, insurance}) => {
    return `http://localhost:8080/api/loan?amount=${amount}&months=${months}&insurance=${insurance}`;
};

export const fetchInstalmentEpic = (action$) => action$.pipe(
    ofType(FETCH_INSTALMENT_REQUEST),
    debounceTime(500),
    switchMap(action => {
        const {amount, months, insurance} = action.payload;
        return ajax.getJSON(url({amount, months, insurance}))
            .pipe(
                map(response => {
                    const {instalment} = response.payload;
                    return fetchInstalmentSuccess(instalment);
                }),
                catchError(error => of(fetchInstalmentFailure(error)))
            );
    })
);

// reducers to be combined
export const isLoadingReducer = (state = false, action) => {
    if (action.type === FETCH_INSTALMENT_REQUEST) return true;
    if (action.type === FETCH_INSTALMENT_SUCCESS) return false;
    return state;
};

export const errorReducer = (state = null, action) => {
    if (action.type === FETCH_INSTALMENT_FAILURE) return action.payload;
    return state;
};

export const dataReducer = (state = null, action) => {
    if (action.type === FETCH_INSTALMENT_SUCCESS) return action.payload;
    return state;
};

// selectors
export const selectInstalment = path(['instalment']);

// reducer
export default combineReducers({
    isLoading: isLoadingReducer,
    error: errorReducer,
    data: dataReducer,
});


