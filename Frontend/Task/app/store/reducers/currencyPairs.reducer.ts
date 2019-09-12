import { Reducer } from 'redux'
import { ConfigurationState } from '../types';

export const types = {
    UPDATE_STATE: '@exchange_rate/UPDATE_STATE',
    FETCH_CONFIGURATION: '@exchange_rate/FETCH_CONFIGURATION',
    FETCH_RATES_POLLING: '@exchange_rate/FETCH_RATES_POLLING',
}

const initialState: ConfigurationState = {
    loading: false,
    currencyPairs: [],
    currencyPairsIdList: []
}

const reducer: Reducer<ConfigurationState> = (state = initialState, action) => {
    switch (action.type) {

        case types.UPDATE_STATE: {
            return {
                ...state,
                ...action.payload
            }
        }

        case types.FETCH_CONFIGURATION: {
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
    updateState: (payload: ConfigurationState) => ({ type: types.UPDATE_STATE, payload }),
    fetchConfiguration: () => ({ type: types.FETCH_CONFIGURATION}),
    fetchRatesPolling: (payload: string[]) => ({ type: types.FETCH_RATES_POLLING, payload})
}

export default reducer;