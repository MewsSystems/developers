import * as actionTypes from './actionTypes.js'
import axios from "../../axios";
import {mapKeys} from 'lodash'
import {fetchRatesFail, fetchRatesStart, fetchRatesSuccess} from "./rates";

export const fetchPairsStart = () => {
    return {
        type: actionTypes.FETCH_PAIRS_START
    }
}

export const fetchPairsSuccess = (pairs) => {

    const pairsJson = JSON.stringify(pairs);
    sessionStorage.setItem('pairs', pairsJson)

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

    const pairsJson = JSON.stringify(pairs);
    sessionStorage.setItem('pairs', pairsJson)

    return {
        type: actionTypes.UPDATE_PAIRS,
        pairs: pairs
    }
}

export const setPairsLinks = (pairsLinks) => {

    const pairsLinksJson = JSON.stringify(pairsLinks);
    sessionStorage.setItem('pairsLinks', pairsLinksJson)

    return {
        type: actionTypes.SET_PAIRS_LINKS,
        pairsLinks: pairsLinks
    }
}

export const fetchPairs = () => {
    return dispatch => {
        dispatch(fetchPairsStart())
        axios.get(`/configuration`)
            .then(res => {
                const arrPairs = []

                mapKeys((res.data.currencyPairs), function (val, key) {
                    arrPairs.push({
                        id: key,
                        pair: val
                    })
                })

                const pairsToShow = []
                arrPairs.map((el, idx) => {
                    el.idx = idx
                    pairsToShow.push(el)
                    return false
                })

                const arrPairsLinks = []
                arrPairs.map((el, idx) => {
                    arrPairsLinks.push([el.id, el.idx])
                    return false
                })

                dispatch(fetchPairsSuccess(pairsToShow))
                dispatch(setPairsLinks(arrPairsLinks))
                dispatch(getRates(arrPairsLinks))

            })
            .catch(
                error => {
                    dispatch(fetchPairsFail(error))
                }
            )
    }
}

export const getRates = (pairsLinks) => {
    return dispatch => {

        setInterval(
            () => {
                dispatch(fetchRatesStart())

                let linkArr = []

                pairsLinks.map(item => {
                    linkArr.push(`currencyPairIds[${item[1]}]=${item[0]}`)
                    return false
                })

                let link = linkArr[0]

                if (linkArr.length > 1) {
                    link = linkArr.join('&')
                }

                axios.get(`/rates?${link}`)
                    .then(res => {

                        const allRates = []

                        mapKeys((res.data.rates), function (val, key) {
                            allRates.push({id: key, coef: val})
                        })

                        dispatch(fetchRatesSuccess(allRates))
                    })
                    .catch(
                        error => {
                            dispatch(fetchRatesFail(error))
                        }
                    )
            }, 5000
        )
    }
}



