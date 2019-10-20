import * as actionTypes from './actionTypes.js'
import axios from "../../axios";
import { mapKeys } from 'lodash'

export const fetchPairsStart = () => {
    return {
        type: actionTypes.FETCH_PAIRS_START
    }
}

export const fetchPairsSuccess = (pairs) => {
    return {
        type: actionTypes.FETCH_PAIRS_SUCCESS,
        pairs: pairs
    }
}

export const fetchPairsFail = (error) => {
    return {
        type: actionTypes.FETCH_PAIRS_FAILED,
        error: error
    }
}

export const updatePairs = (pairs) => {
    return {
        type: actionTypes.UPDATE_PAIRS,
        pairs: pairs
    }
}

export const fetchPairs = () => {
    return dispatch => {
        dispatch(fetchPairsStart())
        axios.get(`/configuration`)
            .then(res => {
                const arrPairs = []              

                mapKeys((res.data.currencyPairs), function(val, key) {
                    arrPairs.push({
                        id: key,
                        pair: val
                    })
                })

                const pairsToShow = []
                arrPairs.map((el, idx) => {
                    el.idx = idx
                    pairsToShow.push(el)
                })

                dispatch(fetchPairsSuccess(pairsToShow))
            })
            .catch(
                error => {
                    dispatch(fetchPairsFail(error))
                }
            )
    }
}
