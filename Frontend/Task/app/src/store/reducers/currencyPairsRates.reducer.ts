import { currencyPairsRatesActionsType } from "../actions/currencyPairsRates.actions";

const initialState = {
    config: {},
    rates: {},
    filters: JSON.parse(localStorage.getItem('filters')) || [],
    configLoading: false,
    configError: false,
    ratesLoading: false,
    ratesError: false,
};

function currencyPairsRatesReducer(state: any = initialState, action:any) {
    switch(action.type) {
        case currencyPairsRatesActionsType.GET_CONFIG_SUCCESS: {
            return {
                ...state,
                config: action.currencyPairs
            }
        }
        case currencyPairsRatesActionsType.GET_CONFIG_LOADING: {
            return {
                ...state,
                configLoading: action.configLoading
            }
        }
        case currencyPairsRatesActionsType.GET_CONFIG_ERROR: {
            return {
                ...state,
                configError: action.configError
            }
        }
        case currencyPairsRatesActionsType.GET_RATES_SUCCESS: {
            return {
                ...state,
                rates: action.rates
            }
        }
        case currencyPairsRatesActionsType.GET_RATES_LOADING: {
            return {
                ...state,
                ratesLoading: action.ratesLoading
            }
        }
        case currencyPairsRatesActionsType.GET_RATES_ERROR: {
            return {
                ...state,
                ratesLoading: action.ratesError,
                ratesError: action.ratesError
            }
        }
        case currencyPairsRatesActionsType.UPDATE_FILTERS: {
            return {
                ...state,
                filters: action.filters
            }
        }
        default: {
            return state
        }
    }
}

export default currencyPairsRatesReducer;