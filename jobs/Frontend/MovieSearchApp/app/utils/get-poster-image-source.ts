type Size = "w92" | "w154" | "w185" | "w342" | "w500" | "w780" | "original"

export const getPosterImageSource = (path: string, size: Size) => {
  const formattedPath = path.replace(/\//g, "")
  return `/resources/poster-image/${size}/${formattedPath}`
}
