export const getMoviePosterDimensions = (width: number) => {
  const ratioPoster = 3 / 2
  const height = width * ratioPoster

  return { width, height }
}
