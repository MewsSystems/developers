import {
	SET_PAIRS,
	SELECT_PAIRS,
	SET_RATES,
} from './constants'

export const setPairs = data => ({
	type: SET_PAIRS,
	payload: data,
})
export const selectPairs = pairs => ({
	type: SELECT_PAIRS,
	payload: pairs,
})
export const setRates = rates => ({
	type: SET_RATES,
	payload: rates,
})