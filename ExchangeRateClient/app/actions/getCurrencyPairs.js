import { keys, prop } from 'ramda';
import Chance from 'chance';
import { apiActionCreator, initRatesHistory, processCurrencyPairs } from './utils';
import { getCurrencyPairs } from '../api';
import { actionTypes } from '../constants';
import { getInterval } from '../selectors';

const dispatchOnSuccess = (dispatch) => ({ data }) => {
    dispatch({ type: actionTypes.GET_CURRENCY_PAIRS_SUCCEEDED, payload: processCurrencyPairs(data) });
    dispatch({ type: actionTypes.INIT_CURRENCY_PAIRS_ORDERING, payload: keys(prop('currencyPairs', data)) });
    dispatch({ type: actionTypes.INIT_RATES_HISTORY, payload: initRatesHistory(data) });
};

const dispatchOnError = (dispatch, getState) => ({ response }) => {
    const state = getState();
    const chance = new Chance();
    const errorId = chance.guid();
    dispatch({
        type: actionTypes.GET_CURRENCY_PAIRS_FAILED,
        payload: { [errorId]: `Encountered the following error while fetching currency pairs: ${response.statusText}` },
    });
    setTimeout(() => dispatch({ type: actionTypes.REMOVE_ERROR_MESSAGE, payload: errorId }), getInterval(state) - 1);
};

export default () => apiActionCreator(
    'GET_CURRENCY_PAIRS',
    getCurrencyPairs(),
    dispatchOnSuccess,
    dispatchOnError,
);
