import { trend } from '../constants'

export default (rates, prevRates) =>
  Object.keys(rates).reduce((newObject, key) => {
    switch (true) {
      case rates[key] > prevRates[key]:
        newObject[key] = trend.GROWING
        break
      case rates[key] < prevRates[key]:
        newObject[key] = trend.DECLINING
        break
      default:
        newObject[key] = trend.STAGNATING
    }

    return newObject
  }, {})
