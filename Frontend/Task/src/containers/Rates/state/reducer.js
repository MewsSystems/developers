import {fromJS} from 'immutable'
import {
	initialState,
	SET_PAIRS,
	SELECT_PAIRS,
	SET_RATES,
} from './constants'
import storage from 'utils/local-storage'

function ratesReducer(state = initialState, action) {
	switch (action.type) {
	case SET_PAIRS:
		return state.set(`pairs`, fromJS(action.payload))
	case SELECT_PAIRS:
		storage.selectedPairs = action.payload
		return state.set(`selectedPairs`, fromJS(action.payload))
	case SET_RATES:
		const rates = fromJS(action.payload)
		return state.withMutations(map => map
			.set(`rates`, rates)
			.update(`ratesHistory`, history => history
				.push(rates)
				.shift()
			)
		)
	default:
		return state
	}
}

export default ratesReducer
