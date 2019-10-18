import { Constants, fetchingPairs, setPairs, clearRateError, setPrevRates, setRates, setRateError } from "./actions";

/**
 * Custom middleware to persist changes to localStorage.
 * We don't need to save the state for every single action, just the important ones.
 */
export const persistChanges = store => next => action => {
    const result = next(action);
    switch (action.type) {
        case Constants.FETCHING_PAIRS:
        case Constants.SET_PAIRS:
        case Constants.TOGGLE_PAIR:
        case Constants.SET_RATES:
            localStorage["exchange"] = JSON.stringify(store.getState());
    }
    return result;
};

/**
 * Thunk middleware for fetching pairs.
 */
export const getPairs = () => dispatch => {
    dispatch(fetchingPairs());

    fetch("http://localhost:3000/configuration")
        .then(result => result.json())
        .then(json => dispatch(setPairs(json)));
};

/**
 * Thunk middleware for polling rates.
 */
export const pollRates = endpoint => (dispatch, getState) => {
    const {
        pairFilter,
        rates: { current }
    } = getState();

    if (pairFilter.length > 0) {
        fetch(endpoint, {
            method: "post",
            body: JSON.stringify({
                currencyPairIds: pairFilter
            }),
            headers: {
                "Content-Type": "application/json"
            }
        })
            .then(result => result.json())
            .then(json => {
                dispatch(clearRateError());
                dispatch(setPrevRates(current));
                dispatch(setRates(json.rates));
            })
            .catch(error => dispatch(setRateError()));
    }
};
