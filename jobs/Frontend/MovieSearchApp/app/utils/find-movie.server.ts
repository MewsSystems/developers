import { getApiKey } from "~/utils/get-api-key.server"

type Options = {
  language?: string
}

type MovieDetails = {
  backdrop_path: string | null
  credits: {
    cast: {
      id: number
      name: string
    }[]
    crew: {
      id: number
      name: string
      job: string
    }[]
  }
  genres: {
    id: number
    name: string
  }[]
  homepage: string | null
  id: number
  overview: string | null
  poster_path: string | null
  release_date: string
  runtime: number | null
  tagline: string | null
  title: string
  vote_average: number
}

export const findMovie = async (id: string, options?: Options) => {
  const apiKey = getApiKey("Opening movie details is not authorized")

  const { language = "en-US" } = options || {}

  const resourceUrl = new URL(`https://api.themoviedb.org/3/movie/${id}`)

  resourceUrl.searchParams.set("append_to_response", "credits")
  resourceUrl.searchParams.set("language", language)
  resourceUrl.searchParams.set("api_key", apiKey)

  const response = await fetch(resourceUrl, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      "Cache-Control": "public, max-age=3600", // 1 hour
    },
  })

  return (await response.json()) as MovieDetails
}
