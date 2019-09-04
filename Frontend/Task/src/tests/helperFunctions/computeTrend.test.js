import { computeTrend } from 'helperFunctions/computeTrend'

const action = jest.fn()

test.only('should return "trend is not available"', () => {
  computeTrend({ prevValue: null, currentValue: null }, action)
  expect(action).toHaveBeenLastCalledWith('trend is not available')
})

test.only('should return "growing"', () => {
  computeTrend({ prevValue: 1, currentValue: 2 }, action)
  expect(action).toHaveBeenLastCalledWith('growing')
})

test.only('should return "declining"', () => {
  computeTrend({ prevValue: 2, currentValue: 1 }, action)
  expect(action).toHaveBeenLastCalledWith('declining')
})

test.only('should return "stagnating"', () => {
  computeTrend({ prevValue: 1, currentValue: 1 }, action)
  expect(action).toHaveBeenLastCalledWith('stagnating')
})