import {fromJS} from 'immutable'
import {
	initialState,
	SET_PAIRS,
	SELECT_PAIRS,
} from './constants'
import storage from 'utils/local-storage'

function ratesReducer(state = initialState, action) {
	switch (action.type) {
	case SET_PAIRS:
		return state.set(`pairs`, fromJS(action.payload))
	case SELECT_PAIRS:
		storage.selectedPairs = action.payload
		return state.set(`selectedPairs`, fromJS(action.payload))
	default:
		return state
	}
}

export default ratesReducer
