import { render, screen } from "@testing-library/react"
import { Provider } from "react-redux"
import { BrowserRouter } from "react-router-dom"
import { store } from "@/app/store"
import MovieDetailPage from "./MovieDetailPage"

const MOVIE_WITH_ALL_INFO = {
  title: "Movie title test",
  overview: "Overview test",
  homepage: "#",
  poster_path: "#",
  genres: [
    { id: 1, name: "Genre 1" },
    { id: 2, name: "Genre 2" },
  ],
  vote_average: 7.5,
  vote_count: 1000,
  popularity: 1000,
  release_date: "2023-12-06",
  status: "Released",
  budget: 12345,
  revenue: 23456,
  belongs_to_collection: { name: "Test collection" },
  production_companies: [
    { id: 1, name: "Company 1" },
    { id: 2, name: "Company 2" },
  ],
  production_countries: [
    { iso_3166_1: "PE", name: "Peru" },
    { iso_3166_1: "ES", name: "Spain" },
  ],
}

const MOVIE_WITHOUT_ALL_INFO = {
  title: "Movie title test",
  overview: "",
  homepage: "#",
  poster_path: "#",
  genres: [],
  vote_average: 0,
  vote_count: 0,
  popularity: 0,
  release_date: undefined,
  status: "Released",
  budget: 0,
  revenue: 0,
  belongs_to_collection: undefined,
  production_companies: [],
  production_countries: [],
}

const mocks = vi.hoisted(() => {
  return {
    useGetMovieQuery: vi.fn(),
  }
})

vi.mock("@/features/api/apiSlice", async (importOriginal) => {
  const mod = await importOriginal<typeof import("@/features/api/apiSlice")>()
  return { ...mod, useGetMovieQuery: mocks.useGetMovieQuery }
})

vi.mock("react-router-dom", async (importOriginal) => {
  const mod = await importOriginal<typeof import("react-router-dom")>()
  return { ...mod, useParams: () => ({ movieId: vi.fn() }) }
})

afterEach(() => {
  vi.clearAllMocks()
})

describe("movie is not found", () => {
  test("error message is shown", () => {
    mocks.useGetMovieQuery.mockReturnValue({
      data: undefined,
      isLoading: false,
      isSuccess: true,
      isError: false,
      error: {},
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <MovieDetailPage />
        </BrowserRouter>
      </Provider>,
    )

    expect(screen.getByText(/movie wasn't found/i)).toBeInTheDocument()
  })
})

describe("movie is loading", () => {
  test("loading message is shown", () => {
    mocks.useGetMovieQuery.mockReturnValue({
      data: undefined,
      isLoading: true,
      isSuccess: false,
      isError: false,
      error: {},
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <MovieDetailPage />
        </BrowserRouter>
      </Provider>,
    )

    expect(screen.getByText(/loading/i)).toBeInTheDocument()
  })
})

describe("there is an error", () => {
  test("error message is shown", () => {
    mocks.useGetMovieQuery.mockReturnValue({
      data: undefined,
      isLoading: false,
      isSuccess: false,
      isError: true,
      error: { status: "", data: { status_message: "error 1" } },
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <MovieDetailPage />
        </BrowserRouter>
      </Provider>,
    )

    expect(screen.getByText(/an error has occurred/i)).toBeInTheDocument()
  })
})

describe("movie is found", () => {
  test("movie has all the information", () => {
    mocks.useGetMovieQuery.mockReturnValue({
      data: MOVIE_WITH_ALL_INFO,
      isLoading: false,
      isSuccess: true,
      isError: false,
      error: {},
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <MovieDetailPage />
        </BrowserRouter>
      </Provider>,
    )

    expect(screen.getByText(/movie title test/i)).toBeInTheDocument()
    expect(screen.getByText(/overview test/i)).toBeInTheDocument()
    expect(screen.getByText(/genre 1/i)).toBeInTheDocument()
    expect(screen.getByText(/genre 2/i)).toBeInTheDocument()
    expect(screen.getByTestId("movieScore")).toHaveTextContent(
      "Score: 7.5 out of 10 (from 1,000 votes)",
    )
    expect(screen.getByTestId("moviePopularity")).toHaveTextContent(
      "Popularity: 1000",
    )
    expect(screen.getByTestId("movieReleaseDate")).toHaveTextContent(
      "Release date: 2023-12-06",
    )
    expect(screen.getByTestId("movieStatus")).toHaveTextContent(
      "Status: Released",
    )
    expect(screen.getByTestId("movieCollection")).toHaveTextContent(
      "Collection: Test collection",
    )
    expect(screen.getByTestId("movieBudget")).toHaveTextContent(
      "Budget: 12,345",
    )
    expect(screen.getByTestId("movieRevenue")).toHaveTextContent(
      "Revenue: 23,456",
    )
    expect(screen.getByTestId("movieProductionCompanies")).toHaveTextContent(
      "Production companies: Company 1, Company 2",
    )
    expect(screen.getByTestId("movieProductionCountries")).toHaveTextContent(
      "Production countries: Peru, Spain",
    )
  })

  test("movie doesn't have all the information", () => {
    mocks.useGetMovieQuery.mockReturnValue({
      data: MOVIE_WITHOUT_ALL_INFO,
      isLoading: false,
      isSuccess: true,
      isError: false,
      error: {},
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <MovieDetailPage />
        </BrowserRouter>
      </Provider>,
    )

    expect(screen.getByText(/movie title test/i)).toBeInTheDocument()
    expect(
      screen.getByText(/there isn't an available overview/i),
    ).toBeInTheDocument()
    expect(screen.getByTestId("movieScore")).toHaveTextContent(
      "Score: Unknown out of 10",
    )
    expect(screen.getByTestId("moviePopularity")).toHaveTextContent(
      "Popularity: Unknown",
    )
    expect(screen.getByTestId("movieReleaseDate")).toHaveTextContent(
      "Release date: Unknown",
    )
    expect(screen.getByTestId("movieStatus")).toHaveTextContent(
      "Status: Released",
    )
    expect(screen.getByTestId("movieCollection")).toHaveTextContent(
      "Collection: Doesn't belong to a collection",
    )
    expect(screen.getByTestId("movieBudget")).toHaveTextContent(
      "Budget: Unknown",
    )
    expect(screen.getByTestId("movieRevenue")).toHaveTextContent(
      "Revenue: Unknown",
    )
    expect(screen.getByTestId("movieProductionCompanies")).toHaveTextContent(
      "Production companies: Unknown",
    )
    expect(screen.getByTestId("movieProductionCountries")).toHaveTextContent(
      "Production countries: Unknown",
    )
  })
})
