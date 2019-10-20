import * as actionTypes from './actionTypes.js'
import axios from "../../axios.js";
import { mapKeys } from 'lodash'

export const fetchRatesStart = () => {
    return {
        type: actionTypes.FETCH_RATES_START
    }
}

export const fetchRatesSuccess = (rates) => {
    return {
        type: actionTypes.FETCH_RATES_SUCCESS,
        rates: rates
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

export const fetchRates = (rates) => {
    if( rates ) {
        return dispatch => {
            dispatch(fetchRatesStart())
    
            let linkArr = []
            
            rates.map(item => {
                linkArr.push(`currencyPairIds[${item.idx}]=${item.id}`)
            })
    
            let link = linkArr[0]

            if (linkArr.length > 1) {
                link = linkArr.join('&')
            }
        
            axios.get(`/rates?${link}`)
                .then(res => {
                     mapKeys((res.data.rates), function(val, key) {
                        rates.map(item => {
                            if (item.id === key) {
                                item.coef = val
                            }
                        })
                    })

                    dispatch(fetchRatesSuccess(rates))
                })
                .catch(
                    error => {
                        dispatch(fetchRatesFail(error))
                    }
                )
        }
    }    
}
