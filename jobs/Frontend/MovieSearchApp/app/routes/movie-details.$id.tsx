import type { LoaderFunctionArgs, MetaFunction } from "@remix-run/node"
import { json } from "@remix-run/node"
import type { ParamParseKey } from "@remix-run/router"
import { useLoaderData } from "@remix-run/react"
import { getMovieTitle } from "~/utils/get-movie-title"
import { findMovie } from "~/utils/find-movie.server"
import { getPosterImageSource } from "~/utils/get-poster-image-source.server"
import { Page } from "~/components/page"
import { getConfigurationDetails } from "~/utils/get-configuration-details.server"
import { Details } from "app/components/details"
import { getFormattedRuntime } from "~/utils/get-formatted-runtime"
import { ErrorDetail } from "~/components/error-detail"
import { useErrorDetails } from "~/utils/use-error-details"

const ROUTE = "movie-details/:id"
type RouteParams = Record<ParamParseKey<typeof ROUTE>, string>

export const loader = async ({ params }: LoaderFunctionArgs) => {
  const { id } = params as RouteParams

  const movieDetails = await findMovie(id)
  const configurationDetails = await getConfigurationDetails()

  const posterImageSrc = getPosterImageSource({
    size: 4,
    imagePath: movieDetails.poster_path,
    configurationDetails,
  })

  return json({
    title: movieDetails.title,
    releaseDate: movieDetails.release_date,
    genres: movieDetails.genres,
    runtime: movieDetails.runtime,
    vote_average: movieDetails.vote_average,
    homepage: movieDetails.homepage,
    tagline: movieDetails.tagline,
    overview: movieDetails.overview,
    credits: movieDetails.credits,
    posterImageSrc,
  })
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

  const title = data.title
  const year = new Date(data.releaseDate).getFullYear()
  const releaseDate = new Date(data.releaseDate).toLocaleDateString("en-US", {
    day: "numeric",
    month: "numeric",
    year: "numeric",
  })
  const genres = data.genres.map((genre) => genre.name).join(", ")
  // format to hours and minutes
  const runtime = getFormattedRuntime(data.runtime)
  const posterImage = data.posterImageSrc
  const tagline = data.tagline
  const overview = data.overview ?? "-"
  const homepage = data.homepage
  const director =
    data.credits.crew.find((crewMember) => /director/i.test(crewMember.job))
      ?.name ?? "Unknown"
  const cast = data.credits.cast.map((castMember) => castMember.name).join(", ")
  const score = data.vote_average

  return (
    <Page>
      <Details
        title={title}
        year={year}
        releaseDate={releaseDate}
        posterImage={posterImage}
        genres={genres}
        runtime={runtime}
        tagline={tagline}
        overview={overview}
        homepage={homepage}
        director={director}
        cast={cast}
        score={score}
      />
    </Page>
  )
}

export function ErrorBoundary() {
  const { statusCode, statusText, message } = useErrorDetails()

  return (
    <Page>
      <ErrorDetail
        message={message}
        statusCode={statusCode}
        statusText={statusText}
      />
    </Page>
  )
}
