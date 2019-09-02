export const generateCurrencyPairs = (currencyData, action) => {
  let currencyPairs = []

  for (const pairKey in currencyData) {
    const mainCurrencyCode = currencyData[pairKey][0].code
    const secondaryCurrencyCode = currencyData[pairKey][1].code
    const currencyPair = {
      name: `${mainCurrencyCode}/${secondaryCurrencyCode}`,
      display: false,
      id: pairKey,
    }
    currencyPairs = [...currencyPairs, currencyPair]
    action(currencyPair)
  }

  return currencyPairs
}