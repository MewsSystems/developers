import type { LoaderFunctionArgs, MetaFunction } from "@remix-run/node"
import { json } from "@remix-run/node"
import {
  Form,
  Link,
  useLoaderData,
  useSearchParams,
  useSubmit,
} from "@remix-run/react"
import { searchMovies, SearchData } from "~/utils/search-movies.server"
import { setNewSearchParams } from "~/utils/set-new-search-params"
import { getPosterImageSource } from "~/utils/get-poster-image-source"

export const loader = async ({ request }: LoaderFunctionArgs) => {
  const url = new URL(request.url)
  const searchQuery = url.searchParams.get("search")
  const page = Number(url.searchParams.get("page") ?? "1")

  if (searchQuery === null || searchQuery === "") {
    const emptySearchResponse: SearchData = {
      page: 1,
      results: [],
      total_pages: 0,
      total_results: 0,
    }

    return json(emptySearchResponse)
  }

  const searchData = await searchMovies(searchQuery, { page })

  return json(searchData, {
    headers: {
      "Cache-Control": "public, max-age=3600", // 3600 seconds = 1 hour
    },
  })
}

export const meta: MetaFunction = () => {
  return [
    { title: "Movie Search App" },
    { name: "description", content: "Simple movie search application" },
  ]
}

export default function Index() {
  const data = useLoaderData<typeof loader>()

  const submit = useSubmit()
  const [searchParams] = useSearchParams()

  const searchQuery = searchParams.get("search") ?? ""
  const page = Number(searchParams.get("page") ?? "1")

  console.log("page", page)

  const handleChange = (event: React.FormEvent<HTMLFormElement>) => {
    submit(event.currentTarget)
  }

  return (
    <main>
      <h1>Find the movie...</h1>
      <Form method={"get"} action={"/"} onChange={handleChange}>
        <label htmlFor="search">Search</label>
        <input
          type="text"
          name="search"
          id="search"
          defaultValue={searchQuery}
        />
        <input type="hidden" name="page" defaultValue={1} />
        <button className={"screen-reader-only"} type="submit">
          Search
        </button>
      </Form>
      <ul>
        {data.results.map((movie) => {
          return (
            <li key={movie.id}>
              <Link to={`/movie-details/${movie.id}`}>
                <h2>{movie.title}</h2>
                <p>{movie.release_date}</p>
                <p>{movie.overview}</p>
                {movie?.poster_path ? (
                  <img
                    src={getPosterImageSource(movie.poster_path, "w92")}
                    alt={`Poster for ${movie.title}`}
                    loading={"lazy"}
                    width={92}
                    height={138}
                  />
                ) : null}
              </Link>
            </li>
          )
        })}
      </ul>
      <section>
        {page === 1 ? (
          <p>Previous</p>
        ) : (
          <Link
            to={{
              search: setNewSearchParams(searchParams, {
                page: `${page - 1}`,
              }).toString(),
            }}
          >
            Previous
          </Link>
        )}
        <Form method={"get"} action={"/"}>
          <input type="hidden" name="search" defaultValue={searchQuery} />
          <section>
            Page{" "}
            <input
              key={page}
              name={"page"}
              type="number"
              defaultValue={page}
              min={1}
              max={data.total_pages}
            />{" "}
            of {data.total_pages}
          </section>
          <button className={"screen-reader-only"} type="submit">
            Go
          </button>
        </Form>
        {page === data.total_pages ? (
          <p>Next</p>
        ) : (
          <Link
            to={{
              search: setNewSearchParams(searchParams, {
                page: `${page + 1}`,
              }).toString(),
            }}
          >
            Next
          </Link>
        )}
      </section>
    </main>
  )
}
