import * as actionTypes from './actionTypes.js'

export const fetchRatesStart = () => {
    return {
        type: actionTypes.FETCH_RATES_START
    }
}

export const fetchRatesSuccess = (allRates) => {
    const allRatesJson = JSON.stringify(allRates);
    sessionStorage.setItem('allRates', allRatesJson)

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

    const ratesJson = JSON.stringify(rates);
    sessionStorage.setItem('rates', ratesJson)

    return {
        type: actionTypes.UPDATE_RATES,
        rates: rates
    }
}


