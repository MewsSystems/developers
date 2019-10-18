import { Constants } from "./actions";
import { combineReducers } from "redux";

const pairs = (state = {}, action) => {
    switch (action.type) {
        case Constants.SET_PAIRS:
            return Object.assign({}, action.payload.currencyPairs);
        default:
            return state;
    }
};

const fetchingPairs = (state = false, action) => {
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

const currentRates = (state = {}, action) => {
    switch (action.type) {
        case Constants.SET_RATES:
            return Object.assign({}, action.payload);
        default:
            return state;
    }
};

const previousRates = (state = {}, action) => {
    switch (action.type) {
        case Constants.SET_PREV_RATES:
            return Object.assign({}, action.payload);
        default:
            return state;
    }
};

const rateError = (state = false, action) => {
    switch (action.type) {
        case Constants.SET_RATE_ERROR:
            return true;
        case Constants.CLEAR_RATE_ERROR:
            return false;
        default:
            return state;
    }
};

export default combineReducers({
    pairs,
    fetchingPairs,
    pairFilter,
    rates: combineReducers({
        current: currentRates,
        previous: previousRates,
        error: rateError
    })
});
