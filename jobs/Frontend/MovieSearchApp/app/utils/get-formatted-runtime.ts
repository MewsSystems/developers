export const getFormattedRuntime = (minutes: number | null) => {
  if (minutes === null) {
    return null
  }

  const hoursValue = Math.floor(minutes / 60)
  const minutesValue = minutes % 60

  if (hoursValue === 0) {
    return `${minutesValue}m`
  }

  if (minutesValue === 0) {
    return `${hoursValue}h`
  }

  return `${hoursValue}h ${minutesValue}m`
}
