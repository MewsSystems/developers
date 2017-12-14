import config from '../../config';

export const SET_RATES = 'SET_RATES';

export function setRates(data) {
    return {
        type: SET_RATES,
        data,
    };
}

export function fetchRates(data) {
    return (dispatch, getState) => {
        const state = getState();
        const ratesIds = Object.keys(state.configuration.data.currencyPairs);
        const query = ratesIds.map(id => 'currencyPairIds[]=' + encodeURIComponent(id)).join('&');

        fetch(`${config.apiUrl}/rates?${query}`, {method: 'GET'})
            .then(response => {
                if (response.status === 500) {
                    throw new Error("Unable to load rates.");
                }

                return response;
            })
            .then(response => response.json())
            .then(data => {
                dispatch(setRates(data));
            })
            .catch(() => {
                // ignore errors, will load later
            });
    }
}
