import { currencyPairsReducer } from 'reducers/currencyPairs'
import { currencyPairs, currencyPair } from 'tests/fixtures/currencyPairs'


test('should add a pair to an empty state', () => {
  const action = {
    type: 'ADD_PAIR', pair: currencyPair
  }
  const state = currencyPairsReducer([], action)
  expect(state).toEqual([action.pair])
})

test('should add a pair to non-empty state', () => {
  const action = {
    type: 'ADD_PAIR', pair: currencyPair
  }
  const state = currencyPairsReducer(currencyPairs, action)
  expect(state).toEqual([...currencyPairs, action.pair])
})

test('should toggle display property', () => {
  const action = {
    type: 'SET_DISPLAY', pairs: ["BDT/VEF"]
  }
  const state = currencyPairsReducer(currencyPairs, action)
  expect(state.find((pair) => pair.name === "BDT/VEF").display).toBe(true)
})