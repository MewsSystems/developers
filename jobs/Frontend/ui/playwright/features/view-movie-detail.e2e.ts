import { expect } from "@playwright/test"
import { http, HttpResponse } from "msw"

import { SearchMovieResponse } from "@/app/features/movie"
import { env } from "@/app/lib/env"
import mockMoviesPageOne from "playwright/data/mockMoviesPageOne.json"
import test from "playwright/fixtures/next-fixture"
import { buildLocalUrl } from "playwright/utils"

const { MOVIE_DB_API_BASE_URL } = env

test("We can view a specific movies details", async ({
  page,
  serverRequestInterceptor,
  port,
}) => {
  await page.goto("/")
  const search = "Marvel"

  serverRequestInterceptor.use(
    http.get(`${MOVIE_DB_API_BASE_URL}/search/movie`, () =>
      HttpResponse.json<SearchMovieResponse>(mockMoviesPageOne),
    ),
  )

  await page.getByLabel("Search").fill(search)

  const theMarvelsMovie = mockMoviesPageOne.results.find(
    (movie) => movie.id === 609681,
  )!

  await page
    .getByRole("link", {
      name: theMarvelsMovie.title,
    })
    .click()

  await page.waitForURL(buildLocalUrl(port, `/movie/${theMarvelsMovie.id}`))

  await expect(
    page.getByRole("heading", {
      name: theMarvelsMovie.title,
    }),
  ).toBeVisible()

  await expect(
    page.getByRole("img", {
      name: theMarvelsMovie.title,
    }),
  ).toBeVisible()

  await expect(page.getByText(theMarvelsMovie.overview)).toBeVisible()
})

test("Search is kept when navigating back to search results", async ({
  page,
  serverRequestInterceptor,
  port,
}) => {
  await page.goto("/")
  const search = "Marvel"

  serverRequestInterceptor.use(
    http.get(`${MOVIE_DB_API_BASE_URL}/search/movie`, () =>
      HttpResponse.json<SearchMovieResponse>(mockMoviesPageOne),
    ),
  )

  await page.getByLabel("Search").fill(search)

  const theMarvelsMovie = mockMoviesPageOne.results.find(
    (movie) => movie.id === 609681,
  )!

  await page
    .getByRole("link", {
      name: theMarvelsMovie.title,
    })
    .click()

  await page.waitForURL(buildLocalUrl(port, `/movie/${theMarvelsMovie.id}`))

  await page.getByRole("button", { name: "Go back to home page" }).click()

  await expect(page.getByLabel("Search")).toHaveValue(search)
})
