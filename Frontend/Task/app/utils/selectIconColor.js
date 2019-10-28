import { trend, PINK, TURQUOISE } from '../constants'

export default currentTrend => {
  switch (currentTrend) {
    case trend.GROWING:
      return TURQUOISE
    case trend.DECLINING:
      return PINK
    default:
      return 'darkgrey'
  }
}
