import {fromJS} from 'immutable'
import {initialState} from '../constants'
import {makeSelectConfig, makeSelectLoading} from '../selectors'
import {testValues} from 'common/constants'

const mockData = fromJS({data: testValues.string})

const mockState = fromJS({
	app: initialState
		.set(`config`, mockData),
})

describe(`App selectors`, () => {
	it(`Should select loading param`, () => {
		const selector = makeSelectLoading()
		expect(selector(mockState)).toEqual(true)
	})
	it(`Should select config param`, () => {
		const selector = makeSelectConfig()
		expect(selector(mockState)).toEqual(mockData)
	})
})
