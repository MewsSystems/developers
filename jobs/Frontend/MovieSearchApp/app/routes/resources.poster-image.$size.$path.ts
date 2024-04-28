import { json, LoaderFunctionArgs } from "@remix-run/node"
import type { ParamParseKey } from "@remix-run/router"
import { getApiKey } from "~/utils/get-api-key.server"

const ROUTE = "resources/poster-image/:size/:path"
type RouteParams = Record<ParamParseKey<typeof ROUTE>, string>

type ConfigurationDetails = {
  images: {
    base_url: string
    secure_base_url: string
    backdrop_sizes: string[]
    logo_sizes: string[]
    poster_sizes: string[]
    profile_sizes: string[]
    still_sizes: string[]
  }
  change_keys: string[]
}

export const loader = async ({ params }: LoaderFunctionArgs) => {
  const apiKey = getApiKey()

  const { path, size } = params as RouteParams

  const resourceUrl = new URL("https://api.themoviedb.org/3/configuration")
  resourceUrl.searchParams.set("api_key", apiKey)

  const configurationDetailsResponse = await fetch(resourceUrl, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      "Cache-Control": "public, max-age=31536000, immutable", // 31536000 seconds = 365 days
    },
  })

  const configurationDetails =
    (await configurationDetailsResponse.json()) as ConfigurationDetails

  const baseUrl = `${configurationDetails.images.secure_base_url}`

  // [ 'w92', 'w154', 'w185', 'w342', 'w500', 'w780', 'original' ]
  const posterSize = configurationDetails.images.poster_sizes.find(
    (posterSize) => posterSize === size
  )

  const imageUrl = `${baseUrl}${posterSize}/${path}`

  const imageBlob = await fetch(imageUrl).then((response) => response.blob())

  return new Response(imageBlob, {
    headers: {
      "Content-Type": imageBlob.type,
      "Content-Length": imageBlob.size.toString(),
      "Content-Disposition": `inline; filename="${path}"`,
      "Cache-Control": "public, max-age=31536000, immutable", // 31536000 seconds = 365 days
    },
  })
}
