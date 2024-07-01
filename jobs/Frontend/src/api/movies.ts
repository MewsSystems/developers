import { MovieResponse } from "@types"

const API_URL = import.meta.env.VITE_THEMOVIEDB_API_URL
const API_KEY = import.meta.env.VITE_THEMOVIEDB_API_KEY

export const fetchMovies = async (
  query: string,
  context: { pageParam: number },
): Promise<MovieResponse> => {
  const url = new URL(`${API_URL}/search/movie`)

  const params = new URLSearchParams({
    query: encodeURIComponent(query),
    page: context.pageParam.toString(),
    api_key: API_KEY,
  })
  url.search = params.toString()

  const response = await fetch(url)
  const json = await response.json()

  return json
}
