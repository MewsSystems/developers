import {SET_CONFIG, SET_LOADING} from './constants'

export const setConfig = config => ({
	type: SET_CONFIG,
	payload: config,
})
export const setLoading = value => ({
	type: SET_LOADING,
	payload: value,
})