import { combineReducers } from 'redux';
import {A} from '../actions'

const endpoint = (endpoint = 'localhost', action = {}) => {
	return endpoint
}

const interval = (interval = 0, action = {}) => {
	return interval
}

const currencyPairs = (pairs = [], action = {}) => {
	switch (action.type) {
		case A.GOT_CONFIG: 
		case A.UPDATE_CONFIG: 
			return action.currencyPairs || pairs
		default:
			return pairs
	}
}

const trendCheckers = (trendCheckers = {}, action = {}) => {
	const stateDefault = {
		stagnating: (prevVal, currVal) => prevVal === currVal,
		growing: (prevVal, currVal) => prevVal < currVal,
		declining: (prevVal, currVal) => prevVal > currVal
	}
	return { ...stateDefault, ...trendCheckers }
}

const activeFilters = (activeFilters = [], action = {}) => {
	if ( action.type == A.ADD_FILTER ) {
		const allFilters = Object.keys(action.allFilters)
		const selectedFilter = allFilters.find(
			filter => filter === action.filter
		)
		return [...activeFilters, selectedFilter]
	}
	if ( action.type == A.REMOVE_FILTER ) {
		return activeFilters.reduce(
			(arr, filter) =>
				(action.filter === filter) ?
					arr :
					[...arr, filter],
			[]
		)
	}
	return activeFilters
}

const rates = (rates = {}, action = {}) => {
	switch (action.type) {
		case A.GOT_RATES:
			return action.rates
		case A.FAILED_RATES: 
		default:
			return rates
	}
}

const configEndpoint = (statePart = '/configuration', action = {}) => { // getting a bit magical here as we don't actually receive that from the config but its in the task
	return statePart
}

const lastAction = (statePart = {}, action = {}) => {
	console.log("Last action:")
	console.log(action)
	return action.type
}

export default combineReducers({ 
	rates,
	interval,
	endpoint,
	lastAction,
	activeFilters,
	currencyPairs,
	configEndpoint,
	trendCheckers
})