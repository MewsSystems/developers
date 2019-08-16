import {RootState} from "../../types/state";

export const initialStore = {
    app: {
        loading: false,
        rates: {},
        currencies: {},
        error: false,
        date: ''
    },
    user: {
        userRates: []
    }
} as RootState;