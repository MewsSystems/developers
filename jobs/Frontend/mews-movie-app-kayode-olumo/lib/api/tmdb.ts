import { TMDB_BASE_URL } from "@/lib/constants"

export async function fetchMovieDetails(id: string) {
  const TMDB_ACCESS_TOKEN = process.env.TMDB_ACCESS_TOKEN
  
  if (!TMDB_ACCESS_TOKEN) {
    throw new Error("TMDB_ACCESS_TOKEN not configured")
  }

  const response = await fetch(`${TMDB_BASE_URL}/movie/${id}`, {
    headers: {
      Authorization: `Bearer ${TMDB_ACCESS_TOKEN}`,
      accept: "application/json",
      "Content-Type": "application/json",
    },
  })

  if (!response.ok) {
    if (response.status === 404) {
      throw new Error("Movie not found")
    }
    throw new Error(`TMDB API error: ${response.status}`)
  }

  return response.json()
}

export async function fetchSearchResults(query: string, page = 1) {
  const TMDB_ACCESS_TOKEN = process.env.TMDB_ACCESS_TOKEN
  
  if (!TMDB_ACCESS_TOKEN) {
    throw new Error("TMDB_ACCESS_TOKEN not configured")
  }

  const params = new URLSearchParams({
    page: page.toString(),
  })

  if (query.trim()) {
    params.append("query", query)
  }

  const endpoint = query.trim()
    ? `/search/movie?${params}`
    : `/movie/popular?${params}`

  const response = await fetch(`${TMDB_BASE_URL}${endpoint}`, {
    headers: {
      Authorization: `Bearer ${TMDB_ACCESS_TOKEN}`,
      "Content-Type": "application/json",
    },
  })

  if (!response.ok) {
    throw new Error(`TMDB API error: ${response.status}`)
  }

  return response.json()
}
