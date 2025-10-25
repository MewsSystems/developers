const TMDB_BASE_URL = "https://api.themoviedb.org/3"

export async function fetchMovieDetails(id: string) {
  const TMDB_API_KEY = process.env.TMDB_API_KEY
  
  if (!TMDB_API_KEY) {
    throw new Error("TMDB_API_KEY not configured")
  }

  const response = await fetch(`${TMDB_BASE_URL}/movie/${id}`, {
    headers: {
      Authorization: `Bearer ${TMDB_API_KEY}`,
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
  const TMDB_API_KEY = process.env.TMDB_API_KEY
  
  if (!TMDB_API_KEY) {
    throw new Error("TMDB_API_KEY not configured")
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
      Authorization: `Bearer ${TMDB_API_KEY}`,
      "Content-Type": "application/json",
    },
  })

  if (!response.ok) {
    throw new Error(`TMDB API error: ${response.status}`)
  }

  return response.json()
}
