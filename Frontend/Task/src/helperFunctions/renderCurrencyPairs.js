export const renderCurrencyPairs = (currencyData) => {
  let currencyPairs = []

  for (const pairKey in currencyData) {
    const mainCurrencyCode = currencyData[pairKey][0].code
    const secondaryCurrencyCode = currencyData[pairKey][1].code
    const shortcutName = `${mainCurrencyCode}/${secondaryCurrencyCode}`
    currencyPairs = [...currencyPairs, shortcutName]

  }

  return currencyPairs
}