/**
 * Builds the full image URL for TMDB images
 * @param path - The image path from TMDB API (can be null)
 * @param size - The image size (default: w500)
 * @returns Full image URL or null if no path provided
 */
export const getImageUrl = (path: string | null, size: string = "w500"): string | null => {
  if (!path) return null
  return `https://image.tmdb.org/t/p/${size}${path}`
}

/**
 * Extracts the year from a release date string
 * @param releaseDate - Date string in format YYYY-MM-DD
 * @returns Year as number
 */
export const getYear = (releaseDate: string): number => {
  return new Date(releaseDate).getFullYear()
}

/**
 * Formats runtime from minutes to hours and minutes
 * @param minutes - Runtime in minutes
 * @returns Formatted string like "2h 30m"
 */
export const formatRuntime = (minutes: number): string => {
  const hours = Math.floor(minutes / 60)
  const mins = minutes % 60
  return `${hours}h ${mins}m`
}

/**
 * Formats numbers with locale-specific separators
 * @param num - Number to format
 * @param locale - Locale string (default: en-US)
 * @returns Formatted number string
 */
export const formatNumber = (num: number, locale: string = "en-US"): string => {
  return num.toLocaleString(locale)
}
