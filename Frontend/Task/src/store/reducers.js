import camelCase from 'camelcase'
import {combineReducers} from 'redux-immutable'

const reExecString = str => {
	const result = /\.\/(.+)\/state\/.+$/.exec(str)

	return result && result.length > 0 && result[1]
		? result[1].trim()
		: null
}

const reducers = {}
const req = require.context(`../containers`, true, /reducer\.js$/)

req.keys().forEach(key => {
	const name = reExecString(key)
	const storeName = name && camelCase(name)

	if (storeName) {
		reducers[storeName] = req(key).default
	}
})

export default combineReducers(reducers)
