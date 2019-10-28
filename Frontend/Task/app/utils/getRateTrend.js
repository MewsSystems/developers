import { trend } from '../constants'

export default (rates, prevRates) => {
  const rateTrends = Object.assign({}, rates)

  Object.keys(rateTrends).map(key => {
    switch (true) {
      case rates[key] > prevRates[key]:
        return (rateTrends[key] = trend.GROWING)
      case rates[key] < prevRates[key]:
        return (rateTrends[key] = trend.DECLINING)
      default:
        return (rateTrends[key] = trend.STAGNATING)
    }
  })

  return rateTrends
}
