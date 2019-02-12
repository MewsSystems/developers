import {fromJS} from 'immutable'
import {initialState, SET_CONFIG, SET_LOADING} from './constants'

function appReducer(state = initialState, action) {
	switch (action.type) {
	case SET_CONFIG:
		return state.set(`config`, fromJS(action.payload))
	case SET_LOADING:
		return state.set(`loading`, action.payload)
	default:
		return state
	}
}

export default appReducer
