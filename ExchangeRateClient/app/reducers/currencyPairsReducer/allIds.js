import { actionTypes } from '../../constants';

export default (initialState = {}, action) => {
    switch (action.type) {
        case actionTypes.INIT_CURRENCY_PAIRS_ORDERING: {
            return action.payload;
        }
        case actionTypes.ORDER_CURRENCY_PAIRS: {
            return action.payload;
        }
        default: return initialState;
    }
};
