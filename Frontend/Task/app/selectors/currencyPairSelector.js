export const getCurrencyPairs = ({ pairReducer: { currencyPairs } }) =>
  currencyPairs
export const getCurrencyPairIds = ({ pairReducer: { currencyPairs } }) =>
  Object.keys(currencyPairs)
export const getPairListState = ({ pairReducer: { pairListState } }) =>
  pairListState
