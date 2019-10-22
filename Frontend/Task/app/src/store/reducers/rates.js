import * as actionTypes from '../actions/actionTypes.js'
import {updateObject} from "../../shared/utility";

export const initialStateRates = {
    rates: [],
    loading: false,
    error: null,
    allRates: []
}

const reducer = (state = initialStateRates, action) => {
    switch (action.type) {
        case actionTypes.FETCH_RATES_START:
            return updateObject(state, {
                loading: true,
                error: null
            })
        case actionTypes.FETCH_RATES_SUCCESS:
            for (let i = 0; i < state.allRates.length; i++) {
                action.allRates[i].coef === state.allRates[i].coef
                    ? action.allRates[i].status = 'same'
                    : action.allRates[i].coef > state.allRates[i].coef
                        ? action.allRates[i].status = 'increase'
                        : action.allRates[i].status = 'decrease'
            }
            return updateObject(state, {
                loading: false,
                allRates: action.allRates,
                error: null
            })
        case actionTypes.FETCH_RATES_FAILED:
            return updateObject(state, {
                loading: false,
                error: action.error
            })
        case actionTypes.UPDATE_RATES:
            return updateObject(state, {
                rates: action.rates,
            })
        default:
            return state
    }
}

export default reducer

