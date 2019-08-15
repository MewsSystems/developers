import {RootState} from "../../types/state";

export const initialStore = {
    app: {
        loading: false,
        rates: {},
        currencies: {}
    },
    user: {
        userRates: []
    }
} as RootState;