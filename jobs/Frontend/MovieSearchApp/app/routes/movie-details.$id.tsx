import type { LoaderFunctionArgs, MetaFunction } from "@remix-run/node"
import { json } from "@remix-run/node"
import type { ParamParseKey } from "@remix-run/router"
import { Link, useLoaderData } from "@remix-run/react"
import { getMovieTitle } from "~/utils/get-movie-title"

const ROUTE = "movie-details/:id"
type RouteParams = Record<ParamParseKey<typeof ROUTE>, string>

export const loader = async ({ params }: LoaderFunctionArgs) => {
  const { id } = params as RouteParams
  console.log(id)

  return json({ title: "Movie Title" })
}

export const meta: MetaFunction<typeof loader> = ({ data }) => {
  const movieTitle = getMovieTitle(data?.title)

  return [
    { title: `Movie Search App | ${movieTitle} Details` },
    { name: "description", content: "Movie details" },
  ]
}

export default function MovieDetails() {
  const data = useLoaderData<typeof loader>()

  const movieTitle = getMovieTitle(data.title)

  return (
    <main>
      <Link to={"/"}>Back to search</Link>
      <h1>{movieTitle}</h1>
    </main>
  )
}
