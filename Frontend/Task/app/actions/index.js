import urlParse from 'url-parse'
import httpBuildQuery from 'http-build-query'
import { calculateTrends } from '../functions'

export const A = { // Actions constants
	FETCH_CONFIG: 'FETCH_CONFIG',
	AWAIT_CONFIG: 'AWAIT_CONFIG',
	GOT_CONFIG: 'GOT_CONFIG',
	UPDATE_CONFIG: 'UPDATE_CONFIG',
	FETCH_RATES: 'FETCH_RATES',
	AWAIT_RATES: 'AWAIT_RATES',
	FAILED_RATES: 'FAILED_RATES',
	GOT_RATES: 'GOT_RATES',
	ADD_FILTER: 'ADD_FILTER',
	REMOVE_FILTER: 'REMOVE_FILTER'
}

export const fetchConfig = () => {
	return (dispatch, getState) => {
		const { configEndpoint, endpoint, currencyPairs } = getState()

		const objectEndpoint = new urlParse(endpoint)
		const configEndpointAbsolute = objectEndpoint.origin + configEndpoint
		const configExisted = Object.keys(currencyPairs).length

		dispatch({ type: A.AWAIT_CONFIG })

		if ( configExisted ) { // possible presaved config from the localStorage
			dispatch({
				type: A.GOT_CONFIG
			})
		}

		let result = fetch(configEndpointAbsolute).then(result => {
			return result.json()
		}).then(data => {
			dispatch({
				type: configExisted ? A.UPDATE_CONFIG : A.GOT_CONFIG,
				currencyPairs: data.currencyPairs
			})
		})
	}
}

export const fetchRates = () => {
	return function request(dispatch, getState) {
		dispatch({ type: A.AWAIT_RATES })
		const { endpoint, currencyPairs, rates, trendCheckers } = getState()

		const queryParameter = 'currencyPairIds' // maybe move to global? ideally to have it from server
		const currencyPairsIds = httpBuildQuery( {[queryParameter]: Object.keys(currencyPairs)} )
		let requestURL = `${endpoint}/?${currencyPairsIds}` 

		fetch(requestURL)
			.then(result => {
				return result.json()
			})
			.then(json => {
				const trendCheckedInjected = Object.keys(json.rates).reduce(
					(obj, item) => {
						const prevVal = rates[item] ? rates[item].value : null
						const currVal = json.rates[item]
						return { ...obj,
									[item]: { value: currVal, trend: calculateTrends(trendCheckers, prevVal, currVal) }
								}
					}, 
				{})
				dispatch({
					type: A.GOT_RATES,
					rates: trendCheckedInjected
				})
			}).catch((e) => {
				console.warn('Couldn\'t get correct response from the server:')
				console.warn(e)
				/* 
					Here we can either invoke the 'request' function recursively as many times as we need and limit it by
					a number of attempts stored in the closure or we can retreat with a failed action and let
					the application decide on how to proceed. Generally it would be a cleaner and more scalable solution to fall
					back with a failing action. That will allow us to take the control over the attempts on the store
					level and avoid isolated logic in this specific action.
				*/
				dispatch({ type: A.FAILED_RATES })
			})
	}
}

export const setFilter = (item, on) => (dispatch, getState) => {
	dispatch({
		type: !on ? A.ADD_FILTER : A.REMOVE_FILTER,
		filter: item,
		allFilters: getState().trendCheckers
	})
}