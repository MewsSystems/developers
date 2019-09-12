import { Reducer } from 'redux'
import { ConfigurationState } from '../types';

export const types = {
    UPDATE_STATE: '@exchange_rate/UPDATE_STATE',
    FETCH_CONFIGURATION: '@exchange_rate/FETCH_CONFIGURATION',
}

const initialState: ConfigurationState = {
    loading: false,
    currencyPairs: []
}

const reducer: Reducer<ConfigurationState> = (state = initialState, action) => {
    switch (action.type) {
        // We Update the aplication state
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
}

export default reducer;