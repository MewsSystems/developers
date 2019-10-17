export const Constants = {
    SET_RATES: "SET_RATES"
};

export const setRates = rates => ({
    type: Constants.SET_RATES,
    payload: rates
});

export const getRates = () => (dispatch, getState) => {
    fetch("http://localhost:3000/configuration")
        .then(result => result.json())
        .then(json => {
            dispatch(setRates(json));
        });
};
