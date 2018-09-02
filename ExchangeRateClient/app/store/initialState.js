import { endpoint, interval } from '../config';

const initialState = {
    config: { endpoint, interval },
    currencyPairs: {
        byId: {},
        allIds: [],
    },
    errorMessages: {},
    ratesHistory: {},
    uiControl: {
        showCountdown: false,
    },
};

export default initialState;
