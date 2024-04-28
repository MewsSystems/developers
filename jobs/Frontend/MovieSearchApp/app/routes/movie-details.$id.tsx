import type { LoaderFunctionArgs, MetaFunction } from "@remix-run/node"
import { json } from "@remix-run/node"
import type { ParamParseKey } from "@remix-run/router"
import { useLoaderData } from "@remix-run/react"
import { getMovieTitle } from "~/utils/get-movie-title"
import { findMovie } from "~/utils/find-movie.server"
import { getPosterImageSource } from "~/utils/get-poster-image-source"

const ROUTE = "movie-details/:id"
type RouteParams = Record<ParamParseKey<typeof ROUTE>, string>

export const loader = async ({ params }: LoaderFunctionArgs) => {
  const { id } = params as RouteParams

  const movieDetails = await findMovie(id)

  return json(movieDetails)
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
  console.log("data", data)

  const movieTitle = getMovieTitle(data.title)

  // Filter director from credits.crew, use regex to match "director" in job
  const director = data.credits.crew.find((crewMember) =>
    /director/i.test(crewMember.job)
  )

  return (
    <main>
      {data.poster_path !== null ? (
        <img
          src={getPosterImageSource(data.poster_path, "w500")}
          width={300}
          height={450}
          alt={`Cover for ${movieTitle}`}
        />
      ) : null}
      <h2>
        {/* Title (Year) */}
        {data.title} ({data.release_date})
      </h2>
      <p>
        {/* Release - Genres - Runtime */}
        {data.release_date} -{" "}
        {data.genres.map((genre) => genre.name).join(", ")} - {data.runtime}m
      </p>
      <p>
        {/* Chart - Homepage*/}
        {data.vote_average} - <a href={data.homepage ?? ""}>Homepage</a>
      </p>
      <p>{data.tagline}</p>
      <p>{data.overview}</p>
      <p>Director: {director?.name}</p>
      <h3>Cast</h3>
      <p>{data.credits.cast.map((castMember) => castMember.name).join(", ")}</p>
    </main>
  )
}
