import {fromJS} from 'immutable'
import {setPairs, selectPairs, setRates} from '../actions'
import {initialState} from '../constants'
import reducer from '../reducer'
import {testValues} from 'common/constants'

const mockData = {data: testValues.string}
const imMockData = fromJS(mockData)
const mockList = [testValues.string]
const imMockList = fromJS(mockList)

describe(`Rates reducer`, () => {
	it(`returns the initial state`, () => {
		expect(reducer(undefined, {})).toEqual(initialState)
	})
	it(`return state with new pairs`, () => {
		const expected = initialState.set(`pairs`, imMockData)
		const action = setPairs(mockData)
		expect(reducer(initialState, action)).toEqual(expected)
	})
	it(`return state with new selectedPairs`, () => {
		const expected = initialState.set(`selectedPairs`, imMockList)
		const action = selectPairs(mockList)
		expect(reducer(initialState, action)).toEqual(expected)
	})
	it(`return state with new rates & history`, () => {
		const expected = initialState
			.set(`rates`, imMockData)
			.update(`ratesHistory`, history => history
				.push(imMockData)
				.shift()
			)
		const action = setRates(mockData)
		expect(reducer(initialState, action)).toEqual(expected)
	})
})
