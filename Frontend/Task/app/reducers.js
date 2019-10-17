import { Constants } from "./actions";
import { combineReducers } from "redux";

const pairs = (state = {}, action) => {
    switch (action.type) {
        case Constants.SET_PAIRS:
            return action.payload.currencyPairs;
        default:
            return state;
    }
};

const fetching = (state = false, action) => {
    switch (action.type) {
        case Constants.FETCHING_PAIRS:
            return true;
        case Constants.SET_PAIRS:
            return false;
        default:
            return state;
    }
};

const pairFilter = (state = [], action) => {
    const { payload } = action;
    switch (action.type) {
        case Constants.TOGGLE_PAIR:
            const foundPair = state.find(pair => pair === payload);
            return foundPair ? state.filter(pair => pair !== payload) : [...state, payload];
        default:
            return state;
    }
};

export default combineReducers({
    pairs,
    fetching,
    pairFilter
});
