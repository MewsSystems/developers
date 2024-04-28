import { getApiKey } from "~/utils/get-api-key.server"

export type ConfigurationDetails = {
  images: {
    secure_base_url: string
    backdrop_sizes: ["w300", "w780", "w1280", "original"]
    poster_sizes: ["w92", "w154", "w185", "w342", "w500", "w780", "original"]
  }
}

export const getConfigurationDetails = async () => {
  const apiKey = getApiKey()

  const resourceUrl = new URL("https://api.themoviedb.org/3/configuration")
  resourceUrl.searchParams.set("api_key", apiKey)

  const configurationDetailsResponse = await fetch(resourceUrl, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      "Cache-Control": "public, max-age=31536000, immutable", // 31536000 seconds = 365 days
    },
  })

  return (await configurationDetailsResponse.json()) as ConfigurationDetails
}
