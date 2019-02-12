import {fromJS} from 'immutable'

// export const SET_CONFIG = `App/SET_CONFIG`
// export const SET_LOADING = `App/SET_LOADING`

export const initialState = fromJS({
	pairs: null,
	loadingPairs: null,
	selectedPairs: null,
	rates: null,
	ratesHistory: null,
})