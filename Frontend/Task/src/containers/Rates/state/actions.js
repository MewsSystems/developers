import {SET_PAIRS, SELECT_PAIRS} from './constants'

export const setPairs = data => ({
	type: SET_PAIRS,
	payload: data,
})
export const selectPairs = pairs => ({
	type: SELECT_PAIRS,
	payload: pairs,
})