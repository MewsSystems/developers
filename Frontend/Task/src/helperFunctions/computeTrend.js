export const computeTrend = ({ prevValue, currentValue }, action) => {
  if (prevValue === null) return action('trend is not available')
  if (prevValue < currentValue) return action('growing')
  if (prevValue > currentValue) return action('declining')
  action('stagnating')
}