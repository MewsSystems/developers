import { actionTypes } from '../../constants';

export default (initialState = {}, action) => {
    switch (action.type) {
        case actionTypes.GET_CURRENCY_PAIRS_SUCCEEDED: {
            return action.payload;
        }
        case actionTypes.GET_CURRENCY_PAIRS_FAILED: {
            return initialState;
        }
        case actionTypes.GET_RATES_SUCCEEDED: {
            return action.payload;
        }
        case actionTypes.GET_RATES_FAILED: {
            return initialState;
        }
        case actionTypes.TOGGLE_TRACKING: {
            return action.payload;
        }
        case actionTypes.COLLECTIVE_TOGGLE_TRACKING: {
            return { ...initialState.byId, ...action.payload };
        }
        default: return initialState;
    }
};
