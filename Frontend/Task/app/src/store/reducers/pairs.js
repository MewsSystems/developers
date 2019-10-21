import * as actionTypes from '../actions/actionTypes.js'
import { updateObject } from "../../shared/utility";

const initialState = {
    pairs: [],
    loading: false,
    error: null,
    pairsLinks: []
}

const reducer = (state = initialState, action) => {
    switch (action.type) {


        case actionTypes.FETCH_PAIRS_START:
            return updateObject(state, {
                loading: true,
                error: null
            })
        case actionTypes.FETCH_PAIRS_SUCCESS:
            return updateObject(state, {
                loading: false,
                pairs: action.pairs,
                error: null
            })
        case actionTypes.FETCH_PAIRS_FAILED:
            return updateObject(state, {
                loading: false,
                error: action.error
            })
        case actionTypes.UPDATE_PAIRS:
            return updateObject(state, {
                pairs: action.pairs
            })
        case actionTypes.SET_PAIRS_LINKS:
            return updateObject(state, {
                pairsLinks: action.pairsLinks
            })


        default:
            return state
    }
}

export default reducer
