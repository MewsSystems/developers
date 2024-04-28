import { getApiKey } from "~/utils/get-api-key.server"

type Options = {
  page: number
}

export type SearchData = {
  page: number
  results: {
    id: number
    overview: string
    poster_path: string | null
    release_date: string
    title: string
    vote_average: number
  }[]
  total_pages: number
  total_results: number
}

export const searchMovies = async (query: string, options?: Options) => {
  const apiKey = getApiKey("Search is not authorized")

  const { page = 1 } = options || {}

  const resourceUrl = new URL("https://api.themoviedb.org/3/search/movie")

  resourceUrl.searchParams.set("query", query)
  resourceUrl.searchParams.set("include_adult", "false")
  resourceUrl.searchParams.set("language", "en-US")
  resourceUrl.searchParams.set("page", String(page))
  resourceUrl.searchParams.set("api_key", apiKey)

  const response = await fetch(resourceUrl, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      "Cache-Control": "public, max-age=3600", // 1 hour
    },
  })

  return (await response.json()) as SearchData
}
