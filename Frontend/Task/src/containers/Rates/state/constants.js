import {fromJS, List} from 'immutable'
import storage from 'utils/local-storage'

export const SET_PAIRS = `Rates/SET_PAIRS`
export const SELECT_PAIRS = `Rates/SELECT_PAIRS`
export const SET_RATES = `Rates/SET_RATES`

export const initialState = fromJS({
	pairs: {},
	selectedPairs: storage.selectedPairs || [],
	rates: {},
	ratesHistory: List().setSize(10),
})