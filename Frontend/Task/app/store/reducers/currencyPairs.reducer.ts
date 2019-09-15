import { Reducer } from 'redux'
import { ApplicationState } from '../types';

export const types = {
    UPDATE_STATE: '@exchange_rate/UPDATE_STATE',
    FETCH_CURRENCY_PAIRS: '@exchange_rate/FETCH_CURRENCY_PAIRS',
    FETCH_RATES_POLLING: '@exchange_rate/FETCH_RATES_POLLING',
}

const initialState: ApplicationState = {
    loading: false,
    currencyPairs: {},
    rates: {},
    currencyPairsIds: []
}

const reducer: Reducer<ApplicationState> = (state = initialState, action) => {
    switch (action.type) {

        case types.UPDATE_STATE: {
            return {
                ...state,
                ...action.payload
            }
        }

        case types.FETCH_CURRENCY_PAIRS: {
            return {
                ...state,
                loading: true
            }
        }

        default: {
            return state
        }
    }
}

export const Actions = {
    updateState: (payload: ApplicationState) => ({ type: types.UPDATE_STATE, payload }),
    fetchCurrencyPairs: () => ({ type: types.FETCH_CURRENCY_PAIRS}),
    fetchRatesPolling: (payload: string[]) => ({ type: types.FETCH_RATES_POLLING, payload})
}

export default reducer;