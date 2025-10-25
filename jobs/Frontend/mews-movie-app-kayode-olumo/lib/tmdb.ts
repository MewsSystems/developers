const TMDB_IMAGE_BASE_URL = "https://image.tmdb.org/t/p"

export const getImageUrl = (path: string | null, size: "w500" | "w780" | "original" = "w500") => {
  if (!path) return "/placeholder.svg?height=750&width=500"
  return `${TMDB_IMAGE_BASE_URL}/${size}${path}`
}