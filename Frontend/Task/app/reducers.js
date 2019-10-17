import { Constants } from "./actions";
import { combineReducers } from "redux";

const rates = (state = {}, action) => {
    switch (action.type) {
        case Constants.SET_RATES: {
            return action.payload.currencyPairs;
        }
        default:
            return state;
    }
};

export default combineReducers({
    rates: rates
});
