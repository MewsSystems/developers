import { generateCurrencyPairs } from 'helperFunctions/generateCurrencyPairs'
import { currencyData, currencyPairs } from 'tests/fixtures/currencyPairs'

const action = jest.fn()

test('should convert configuration to array with currency pairs', () => {
  const convertedData = generateCurrencyPairs(currencyData, action)
  expect(convertedData).toEqual(currencyPairs)
  expect(action).toHaveBeenLastCalledWith(currencyPairs[1])
})