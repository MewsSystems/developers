export const Constants = {
    SET_PAIRS: "SET_PAIRS",
    FETCHING_PAIRS: "FETCHING_PAIRS",
    TOGGLE_PAIR: "TOGGLE_PAIR",

    SET_RATES: "SET_RATES",
    SET_PREV_RATES: "SET_PREV_RATES",
    SET_RATE_ERROR: "SET_RATE_ERROR",
    CLEAR_RATE_ERROR: "CLEAR_RATE_ERROR"
};

export const setPairs = pairs => ({
    type: Constants.SET_PAIRS,
    payload: pairs
});

export const fetchingPairs = () => ({
    type: Constants.FETCHING_PAIRS
});

export const togglePair = pair => ({
    type: Constants.TOGGLE_PAIR,
    payload: pair
});

export const setRates = rates => ({
    type: Constants.SET_RATES,
    payload: rates
});

export const setPrevRates = prevRates => ({
    type: Constants.SET_PREV_RATES,
    payload: prevRates
});

export const setRateError = () => ({
    type: Constants.SET_RATE_ERROR
});

export const clearRateError = () => ({
    type: Constants.CLEAR_RATE_ERROR
});
