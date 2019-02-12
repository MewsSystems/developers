import {fromJS} from 'immutable'
import {setConfig, setLoading} from '../actions'
import {initialState} from '../constants'
import reducer from '../reducer'
import {testValues} from 'common/constants'

describe(`App reducer`, () => {
	it(`returns the initial state`, () => {
		expect(reducer(undefined, {})).toEqual(initialState)
	})
	it(`return state with changed config`, () => {
		const mockData = {data: testValues.string}
		const expected = initialState.set(`config`, fromJS(mockData))
		const action = setConfig(mockData)
		expect(reducer(initialState, action)).toEqual(expected)
	})
	it(`return state with loading:false`, () => {
		const expected = initialState.set(`loading`, false)
		const action = setLoading(false)
		expect(reducer(initialState, action)).toEqual(expected)
	})
})
