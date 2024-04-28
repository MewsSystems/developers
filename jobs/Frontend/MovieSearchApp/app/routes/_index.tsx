import type { LoaderFunctionArgs, MetaFunction } from "@remix-run/node"
import { json } from "@remix-run/node"
import { useLoaderData } from "@remix-run/react"
import { searchMovies } from "~/utils/search-movies.server"
import { Search } from "~/components/search"
import { Pagination } from "~/components/pagination"
import { List } from "~/components/list"
import { ListItem } from "~/components/list-item"
import { PreviewCard } from "~/components/preview-card"
import { Page } from "~/components/page"
import { PAGE_PARAM_NAME, SEARCH_PARAM_NAME } from "~/constants"
import { EmptyList } from "~/components/empty-list"
import { getConfigurationDetails } from "~/utils/get-configuration-details.server"
import { getPosterImageSource } from "~/utils/get-poster-image-source.server"
import { useErrorDetails } from "~/utils/use-error-details"
import { ErrorDetail } from "~/components/error-detail"

export const loader = async ({ request }: LoaderFunctionArgs) => {
  const url = new URL(request.url)
  const searchQuery = url.searchParams.get(SEARCH_PARAM_NAME)
  const page = Number(url.searchParams.get(PAGE_PARAM_NAME) ?? "1")

  if (searchQuery === null || searchQuery === "") {
    return json({
      page: 1,
      totalPages: 1,
      totalResults: 0,
      results: [],
    })
  }

  const searchData = await searchMovies(searchQuery, { page })
  const configurationDetails = await getConfigurationDetails()

  const results = searchData.results.map((movie) => {
    const posterImageSrc = getPosterImageSource({
      size: 1,
      imagePath: movie.poster_path,
      configurationDetails,
    })
    return {
      id: movie.id,
      title: movie.title,
      releaseDate: movie.release_date,
      overview: movie.overview,
      posterImageSrc,
    }
  })

  return json(
    {
      page: searchData.page,
      totalPages: searchData.total_pages,
      totalResults: searchData.total_results,
      results,
    },
    {
      headers: {
        "Cache-Control": "public, max-age=3600", // 3600 seconds = 1 hour
      },
    }
  )
}

export const meta: MetaFunction = () => {
  return [
    { title: "Movie Search App" },
    { name: "description", content: "Simple movie search application" },
  ]
}

export default function Index() {
  const { totalPages, totalResults, results } = useLoaderData<typeof loader>()

  return (
    <Page>
      <Search />

      {totalResults === 0 ? (
        <EmptyList />
      ) : (
        <List>
          {results.map((movie) => (
            <ListItem key={movie.id}>
              <PreviewCard
                id={movie.id}
                title={movie.title}
                releaseDate={movie.releaseDate}
                overview={movie.overview ?? "-"}
                posterImage={movie.posterImageSrc}
              />
            </ListItem>
          ))}
        </List>
      )}

      {totalResults !== 0 && <Pagination totalPages={totalPages} />}
    </Page>
  )
}

export function ErrorBoundary() {
  const { statusCode, statusText, message } = useErrorDetails()

  return (
    <Page>
      <Search />

      <ErrorDetail
        message={message}
        statusCode={statusCode}
        statusText={statusText}
      />
    </Page>
  )
}
