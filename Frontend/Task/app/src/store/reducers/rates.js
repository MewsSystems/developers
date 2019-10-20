import * as actionTypes from '../actions/actionTypes.js'
import { updateObject } from "../../shared/utility";

const initialState = {
    rates: [],
    loading: false,
    error: null
}

const reducer = (state = initialState, action) => {
    switch (action.type) {


        case actionTypes.FETCH_RATES_START:
            return updateObject(state, {
                loading: true,
                error: null
            })
        case actionTypes.FETCH_RATES_SUCCESS:
            return updateObject(state, {
                loading: false,
                rates: action.rates,
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