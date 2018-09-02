import { dissoc } from 'ramda';
import { actionTypes } from '../constants';

export default (initialState = {}, action) => {
    switch (action.type) {
        case actionTypes.GET_CURRENCY_PAIRS_FAILED: {
            return { ...initialState, ...action.payload };
        }
        case actionTypes.GET_RATES_FAILED: {
            return { ...initialState, ...action.payload };
        }
        case actionTypes.REMOVE_ERROR_MESSAGE: {
            return dissoc(action.payload, initialState);
        }
        default: return initialState;
    }
};
