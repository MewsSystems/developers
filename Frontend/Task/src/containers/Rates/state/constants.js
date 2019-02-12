import {fromJS} from 'immutable'
import storage from 'utils/local-storage'

export const SET_PAIRS = `Rates/SET_PAIRS`
export const SELECT_PAIRS = `Rates/SELECT_PAIRS`

export const initialState = fromJS({
	pairs: {},
	loadingPairs: [],
	selectedPairs: storage.selectedPairs || [],
	rates: {},
	ratesHistory: [],
})