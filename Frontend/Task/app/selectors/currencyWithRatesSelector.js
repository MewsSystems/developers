import { combineRatesWithPairs } from '../utils'

export const getCurrenciesWithRates = ({
  pairReducer: { currencyPairs },
  rateReducer: { rates, trends },
}) => combineRatesWithPairs(currencyPairs, rates, trends)
