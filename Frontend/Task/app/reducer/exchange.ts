import { ExchangeInterface } from '../interface/exchangeInterface';
import { ExchangeAction, ExchangeActionTypes } from '../interface/exchangeActionInterface';

const initialState = {
    loading: false,
    error: false,
};

export default (state: ExchangeInterface = initialState, action: ExchangeActionTypes) => {
    const { type, payload } = action;

    switch (type) {
        case ExchangeAction.SET_SELECTED:
            return {...state, selectedPair: payload};
        case ExchangeAction.ERROR_TOGGLE:
            return {...state, error: typeof payload === 'boolean' ? payload : !state.error};
        case ExchangeAction.LOADING_TOGGLE:
            const { error = false, loading = false } = state;
            const isLoading = typeof payload === 'boolean' ? payload : !loading;

            return {
                ...state,
                loading: isLoading,
                error: (error && isLoading) ? false : error,
            };
        case ExchangeAction.GET_PAIRS:
            return {...state, pairs: payload};
        case ExchangeAction.ADD_MESSAGE:
            return {...state, message: payload};
        default:
            return state;
    }
};
