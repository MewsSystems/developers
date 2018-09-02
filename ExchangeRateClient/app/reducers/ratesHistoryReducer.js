import { actionTypes } from '../constants';

export default (initialState = {}, action) => {
    switch (action.type) {
        case actionTypes.INIT_RATES_HISTORY: {
            return action.payload;
        }
        case actionTypes.UPDATE_RATES_HISTORY: {
            return action.payload;
        }
        default: return initialState;
    }
};
