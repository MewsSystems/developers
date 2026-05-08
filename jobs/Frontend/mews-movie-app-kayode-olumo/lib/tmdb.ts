import { TMDB_IMAGE_BASE_URL, TMDB_POSTER_SIZE } from "@/lib/constants"

export const getImageUrl = (path: string | null, size: "w500" | "w780" | "original" = TMDB_POSTER_SIZE) => {
  if (!path) return "/placeholder.svg?height=750&width=500"
  return `${TMDB_IMAGE_BASE_URL}/${size}${path}`
}