import Chance from 'chance';
import { apiActionCreator, mergeRatesWithCurrencyPairs, updateRatesHistory } from './utils';
import { getRates } from '../api';
import { actionTypes } from '../constants';
import { getInterval } from '../selectors';


const dispatchOnSuccess = (dispatch, getState) => ({ data }) => {
    const state = getState();
    dispatch({ type: actionTypes.GET_RATES_SUCCEEDED, payload: mergeRatesWithCurrencyPairs(state)(data) });
    dispatch({ type: actionTypes.UPDATE_RATES_HISTORY, payload: updateRatesHistory(state)(data) });
};

const dispatchOnError = (dispatch, getState) => ({ response }) => {
    const state = getState();
    const chance = new Chance();
    const errorId = chance.guid();
    dispatch({
        type: actionTypes.GET_RATES_FAILED,
        payload: { [errorId]: `Encountered the following error while fetching rates: ${response.statusText}` },
    });
    setTimeout(() => dispatch({ type: actionTypes.REMOVE_ERROR_MESSAGE, payload: errorId }), getInterval(state) - 1);
};

export default (currencyPairIds) => apiActionCreator(
    'GET_RATES',
    getRates(currencyPairIds),
    dispatchOnSuccess,
    dispatchOnError,
);
