export default (currencyPairs, rates, trends) =>
  Object.keys(currencyPairs).reduce((newObject, key) => {
    newObject[key] = {
      name: `${currencyPairs[key][0].code}/${currencyPairs[key][1].code}`,
      value: rates[key],
      trend: trends[key],
    }

    return newObject
  }, {})
