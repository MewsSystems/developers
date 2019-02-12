import {fromJS} from 'immutable'
import {initialState} from '../constants'
import {
	makeSelectPairs,
	makeSelectSelectedPairs,
	makeSelectPreparedRates,
} from '../selectors'
import {testValues} from 'common/constants'

const pairs = {}
pairs[testValues.string] = {
	name: testValues.string,
	shortName: testValues.string,
}
const mockPairs = fromJS(pairs)
const mockSelectedPairs = fromJS([testValues.string])
const rates = {}
rates[testValues.string] = 1
const mockRates = fromJS(rates)
const mockHistory = fromJS([mockRates, mockRates])

const mockState = fromJS({
	rates: initialState
		.set(`pairs`, mockPairs)
		.set(`rates`, mockRates)
		.set(`selectedPairs`, mockSelectedPairs)
		.set(`ratesHistory`, mockHistory)
})

describe(`Rates selectors`, () => {
	it(`Should select pairs`, () => {
		const selector = makeSelectPairs()
		expect(selector(mockState)).toEqual(mockPairs)
	})
	it(`Should select selectedPairs`, () => {
		const selector = makeSelectSelectedPairs()
		expect(selector(mockState)).toEqual(mockSelectedPairs)
	})
	it(`Should select prepared rates`, () => {
		const expected = fromJS([{
			current: 1,
			last: 1,
			pair: mockPairs.get(testValues.string),
		}])
		const selector = makeSelectPreparedRates()
		expect(selector(mockState)).toEqual(expected)
	})
})
