import {setConfig, setLoading} from '../actions'
import {SET_CONFIG, SET_LOADING} from '../constants'
import {testValues} from 'common/constants'

describe(`App actions`, () => {
	it(`Should return a type of ${SET_CONFIG} & payload`, () => {
		const expected = {
			type: SET_CONFIG,
			payload: testValues.string,
		}
		expect(setConfig(testValues.string)).toEqual(expected)
	})
	it(`Should return a type of ${SET_LOADING} & payload`, () => {
		const expected = {
			type: SET_LOADING,
			payload: testValues.string,
		}
		expect(setLoading(testValues.string)).toEqual(expected)
	})
})
