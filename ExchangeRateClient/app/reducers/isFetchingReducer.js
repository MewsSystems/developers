import { assoc } from 'ramda';
import { actionTypes } from '../constants';

export default (initialState = {}, action) => {
    switch (action.type) {
        case actionTypes.GET_CURRENCY_PAIRS_STARTED: {
            return assoc('currencyPairs', true, initialState);
        }
        case actionTypes.GET_CURRENCY_PAIRS_SUCCEEDED:
        case actionTypes.GET_CURRENCY_PAIRS_FAILED: {
            return assoc('currencyPairs', false, initialState);
        }
        case actionTypes.GET_RATES_STARTED: {
            return assoc('rates', true, initialState);
        }
        case actionTypes.GET_RATES_SUCCEEDED:
        case actionTypes.GET_RATES_FAILED: {
            return assoc('rates', false, initialState);
        }
        default: return initialState;
    }
};
