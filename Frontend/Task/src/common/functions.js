import {fromJS} from 'immutable'
import randomString from 'randomstring'

export const setValues = (map, data) => {
	let result = map
	Object.keys(data).forEach(key => {
		result = result.set(key, fromJS(data[key]))
	})
	return result
}

export const getKey = (length = 6) => randomString.generate(length)

export const getDefaultArray = (length, defaultValue = null) => {
	const result = []
	if (length > -1) {
		result.length = length
	}
	result.fill(defaultValue)
	return result
}

export const getIntValue = (str, radix = 10) => {
	const intStr = str.match(/\d+/)
	return intStr[0] !== `` ? parseInt(intStr[0], radix) : NaN
}