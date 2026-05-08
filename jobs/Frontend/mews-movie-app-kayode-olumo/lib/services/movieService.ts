import type { MoviesResponse } from "@/types"

export async function searchMovies(query: string, page = 1): Promise<MoviesResponse> {
  const params = new URLSearchParams({
    page: page.toString(),
  })

  if (query.trim()) {
    params.append("query", query)
  }

  const response = await fetch(`/api/search?${params}`)

  if (!response.ok) {
    throw new Error("Failed to search movies")
  }

  return response.json()
}
