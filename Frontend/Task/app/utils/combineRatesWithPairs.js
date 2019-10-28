export default (currencyPairs, rates, trends) => {
  const currenciesWithRates = {}

  Object.keys(currencyPairs).forEach(
    key =>
      (currenciesWithRates[key] = {
        name: `${currencyPairs[key][0].code}/${currencyPairs[key][1].code}`,
        value: rates[key],
        trend: trends[key],
      })
  )

  return currenciesWithRates
}
