import * as actionTypes from './actionTypes.js'

export const fetchRatesStart = () => {
    return {
        type: actionTypes.FETCH_RATES_START
    }
}

export const fetchRatesSuccess = (allRates) => {
    return {
        type: actionTypes.FETCH_RATES_SUCCESS,
        allRates: allRates
    }
}

export const fetchRatesFail = (error) => {
    return {
        type: actionTypes.FETCH_RATES_FAILED,
        error: error
    }
}

export const updateRates = (rates) => {
    return {
        type: actionTypes.UPDATE_RATES,
        rates: rates
    }
}


