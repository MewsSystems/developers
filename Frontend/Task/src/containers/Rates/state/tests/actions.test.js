import {
	SET_PAIRS,
	SELECT_PAIRS,
	SET_RATES,
} from '../constants'
import {setPairs, selectPairs, setRates} from '../actions'
import {testValues} from 'common/constants'

describe(`Rates actions`, () => {
	it(`Should return a type of ${SET_PAIRS} & payload`, () => {
		const expected = {
			type: SET_PAIRS,
			payload: testValues.string,
		}
		expect(setPairs(testValues.string)).toEqual(expected)
	})
	it(`Should return a type of ${SELECT_PAIRS} & payload`, () => {
		const expected = {
			type: SELECT_PAIRS,
			payload: testValues.string,
		}
		expect(selectPairs(testValues.string)).toEqual(expected)
	})
	it(`Should return a type of ${SET_RATES} & payload`, () => {
		const expected = {
			type: SET_RATES,
			payload: testValues.string,
		}
		expect(setRates(testValues.string)).toEqual(expected)
	})
})
