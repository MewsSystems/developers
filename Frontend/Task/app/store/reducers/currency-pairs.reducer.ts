import { Reducer } from 'redux'
import { CurrencyState } from '../types';

export const types = {
    UPDATE_STATE: '@currency_pairs/UPDATE_STATE',
    FETCH_CURRENCY_PAIRS: '@currency_pairs/FETCH_CURRENCY_PAIRS',
    FETCH_RATES_POLLING: '@currency_pairs/FETCH_RATES_POLLING',
}

const initialState: CurrencyState = {
    loading: false,
    currencyPairs: {},
    currencyPairsIds: []
}

const reducer: Reducer<CurrencyState> = (state = initialState, action) => {
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
    updateState: (payload: CurrencyState) => ({ type: types.UPDATE_STATE, payload }),
    fetchCurrencyPairs: () => ({ type: types.FETCH_CURRENCY_PAIRS}),
    fetchRatesPolling: (payload: string[]) => ({ type: types.FETCH_RATES_POLLING, payload})
}

export default reducer;