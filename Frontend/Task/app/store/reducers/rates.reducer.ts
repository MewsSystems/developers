import { Reducer } from 'redux'
import { RatesState } from '../types';

export const types = {
    FETCH_RATES_POLLING: '@rates/FETCH_RATES_POLLING',
    UPDATE_STATE: '@rates/UPDATE_STATE',
}

const initialState: RatesState = {
    rates: {}
}

const reducer: Reducer<RatesState> = (state = initialState, action) => {
    switch (action.type) {

        case types.UPDATE_STATE: {
            return {
                ...state,
                ...action.payload
            }
        }

        default: {
            return state
        }
    }
}

export const Actions = {
    fetchRatesPolling: (payload: string[]) => ({ type: types.FETCH_RATES_POLLING, payload}),
    updateState: (payload: RatesState) => ({ type: types.UPDATE_STATE, payload }),
}

export default reducer;