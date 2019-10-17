export const Constants = {
    SET_PAIRS: "SET_PAIRS",
    FETCHING_PAIRS: "FETCHING_PAIRS",
    TOGGLE_PAIR: "TOGGLE_PAIR"
};

export const setPairs = pairs => ({
    type: Constants.SET_PAIRS,
    payload: pairs
});

export const fetchingPairs = () => ({
    type: Constants.FETCHING_PAIRS
});

export const getPairs = () => (dispatch, getState) => {
    dispatch(fetchingPairs());

    fetch("http://localhost:3000/configuration")
        .then(result => result.json())
        .then(json => dispatch(setPairs(json)));
};

export const togglePair = pair => ({
    type: Constants.TOGGLE_PAIR,
    payload: pair
});
