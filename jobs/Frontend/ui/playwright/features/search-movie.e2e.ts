import { expect } from "@playwright/test"
import { http, HttpResponse } from "msw"

import { SearchMovieResponse } from "@/app/features/movie"
import { env } from "@/app/lib/env"
import mockMoviesPageOne from "playwright/data/mockMoviesPageOne.json"
import mockMoviesPageTwo from "playwright/data/mockMoviesPageTwo.json"
import test from "playwright/fixtures/next-fixture"

const { MOVIE_DB_API_BASE_URL } = env

test("We can search for a movie", async ({
  page,
  serverRequestInterceptor,
}) => {
  await page.goto("/")
  const search = "Marvel"

  const searchMovieHandler = http.get(
    `${MOVIE_DB_API_BASE_URL}/search/movie`,
    ({ request }) => {
      const url = new URL(request.url)
      const query = url.searchParams.get("query")

      if (query !== search) {
        return new HttpResponse(null, { status: 404 })
      }
      return HttpResponse.json<SearchMovieResponse>(mockMoviesPageOne)
    },
  )

  serverRequestInterceptor.use(searchMovieHandler)

  await page.getByLabel("Search").fill(search)

  for (const movie of mockMoviesPageOne.results) {
    await expect(
      page.getByRole("heading", { name: movie.title, exact: true }),
    ).toBeVisible()

    await expect(
      page.getByRole("img", { name: movie.title, exact: true }),
    ).toBeVisible()
  }

  expect(searchMovieHandler.isUsed).toBe(true)
})

test("We can view additional movies with paginated search results", async ({
  page,
  serverRequestInterceptor,
}) => {
  await page.goto("/")
  const search = "Marvel"

  serverRequestInterceptor.use(
    http.get(`${MOVIE_DB_API_BASE_URL}/search/movie`, ({ request }) => {
      const url = new URL(request.url)
      const page = url.searchParams.get("page")

      if (page === "1") {
        return HttpResponse.json<SearchMovieResponse>(mockMoviesPageOne)
      }

      return HttpResponse.json<SearchMovieResponse>(mockMoviesPageTwo)
    }),
  )

  await page.getByLabel("Search").fill(search)

  for (const movie of mockMoviesPageOne.results) {
    await expect(
      page.getByRole("heading", { name: movie.title, exact: true }),
    ).toBeVisible()

    await expect(
      page.getByRole("img", { name: movie.title, exact: true }),
    ).toBeVisible()
  }

  await page.getByRole("button", { name: "Load More Movies" }).click()

  for (const movie of mockMoviesPageTwo.results) {
    await expect(
      page.getByRole("heading", { name: movie.title, exact: true }),
    ).toBeVisible()

    await expect(
      page.getByRole("img", { name: movie.title, exact: true }),
    ).toBeVisible()
  }
})
