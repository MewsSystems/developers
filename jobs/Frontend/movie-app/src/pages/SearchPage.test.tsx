import { render, screen } from "@testing-library/react"
import userEvent from "@testing-library/user-event"
import { Provider } from "react-redux"
import { BrowserRouter } from "react-router-dom"
import { store } from "@/app/store"
import SearchPage from "./SearchPage"

const EMPTY_MOVIE_LIST = {
  page: 1,
  total_results: 0,
  total_pages: 1,
  results: [],
}

const MOVIE_LIST_WITH_ONE_PAGE = {
  page: 1,
  total_results: 2,
  total_pages: 1,
  results: [
    { id: 1, title: "Movie 1", poster_path: "/movies/1" },
    { id: 2, title: "Movie 2", poster_path: "/movies/2" },
  ],
}

const MOVIE_LIST_WITH_MULTIPLE_PAGES = {
  page: 1,
  total_results: 25,
  total_pages: 2,
  results: Array.from({ length: 20 }, (_, i) => ({
    id: i + 1,
    title: `Movie ${i + 1}`,
    poster_path: `/movies/${i + 1}`,
  })),
}

const MOVIE_LIST_WITH_MULTIPLE_PAGES_2 = {
  page: 2,
  total_results: 25,
  total_pages: 2,
  results: Array.from({ length: 5 }, (_, i) => ({
    id: i + 1 + 20,
    title: `Movie ${i + 1 + 20}`,
    poster_path: `/movies/${i + 1 + 20}`,
  })),
}

const mocks = vi.hoisted(() => {
  return {
    useGetMoviesQuery: vi.fn(),
    useSearchParams: vi.fn(),
  }
})

vi.mock("@/features/api/apiSlice", async (importOriginal) => {
  const mod = await importOriginal<typeof import("@/features/api/apiSlice")>()
  return { ...mod, useGetMoviesQuery: mocks.useGetMoviesQuery }
})

describe("search input is empty", () => {
  test("it renders and input placeholder is shown", () => {
    mocks.useGetMoviesQuery.mockReturnValue({
      data: undefined,
      isLoading: true,
      isSuccess: false,
      isFetching: false,
      isError: false,
      error: {},
    })
    const { container } = render(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    expect(container).toBeInTheDocument()
    expect(screen.getByPlaceholderText(/search a movie/i)).toBeInTheDocument()
  })
})

describe("search input isn't empty and is loading", () => {
  test("useGetMoviesQuery is loading", async () => {
    mocks.useGetMoviesQuery.mockReturnValue({
      data: undefined,
      isLoading: true,
      isSuccess: false,
      isFetching: false,
      isError: false,
      error: {},
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    await userEvent.type(screen.getByTestId("searchInput"), "test")
    expect(screen.getByText(/loading/i)).toBeInTheDocument()
    await userEvent.clear(screen.getByTestId("searchInput"))
  })

  test("useGetMoviesQuery is fetching and there is no previous data", async () => {
    mocks.useGetMoviesQuery.mockReturnValue({
      data: EMPTY_MOVIE_LIST,
      isLoading: false,
      isSuccess: false,
      isFetching: true,
      isError: false,
      error: {},
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    await userEvent.type(screen.getByTestId("searchInput"), "test")
    expect(screen.getByText(/loading/i)).toBeInTheDocument()
    await userEvent.clear(screen.getByTestId("searchInput"))
  })
})

describe("search input isn't empty and there aren't results", () => {
  test("show no results message", async () => {
    mocks.useGetMoviesQuery.mockReturnValue({
      data: EMPTY_MOVIE_LIST,
      isLoading: false,
      isSuccess: true,
      isFetching: false,
      isError: false,
      error: {},
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    await userEvent.type(screen.getByTestId("searchInput"), "test")
    expect(screen.getByText(/there are no results/i)).toBeInTheDocument()
    await userEvent.clear(screen.getByTestId("searchInput"))
  })
})

describe("search input isn't empty and error occurred", () => {
  test("error message is shown", async () => {
    mocks.useGetMoviesQuery.mockReturnValue({
      data: undefined,
      isLoading: false,
      isSuccess: false,
      isFetching: false,
      isError: true,
      error: { status: "", data: { status_message: "error 1" } },
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    await userEvent.type(screen.getByTestId("searchInput"), "test")
    expect(screen.getByText(/an error has occurred/i)).toBeInTheDocument()
    await userEvent.clear(screen.getByTestId("searchInput"))
  })
})

describe("search input isn't empty and is success", () => {
  afterEach(() => {
    vi.clearAllMocks()
  })
  test("it shows results when typing", async () => {
    mocks.useGetMoviesQuery.mockReturnValue({
      data: MOVIE_LIST_WITH_ONE_PAGE,
      isLoading: false,
      isSuccess: true,
      isFetching: false,
      isError: false,
      error: {},
    })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    await userEvent.type(screen.getByTestId("searchInput"), "test")

    expect(screen.getByText(/showing 1 to 2 of 2 entries/i)).toBeInTheDocument()
    expect(screen.getByText(/page 1 of 1/i)).toBeInTheDocument()
    expect(screen.getByRole("link", { name: /movie 1/i })).toHaveAttribute(
      "href",
      "/movies/1",
    )
    expect(screen.getByRole("link", { name: /movie 2/i })).toHaveAttribute(
      "href",
      "/movies/2",
    )
    expect(screen.getByTestId("paginationPrevious")).toBeDisabled()
    expect(screen.getByTestId("paginationNext")).toBeDisabled()
    await userEvent.clear(screen.getByTestId("searchInput"))
  })

  test("results are paginated", async () => {
    mocks.useGetMoviesQuery.mockReturnValue({
      data: MOVIE_LIST_WITH_MULTIPLE_PAGES,
      isLoading: false,
      isSuccess: true,
      isFetching: false,
      isError: false,
      error: {},
    })

    const { rerender } = render(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    await userEvent.type(screen.getByTestId("searchInput"), "test")

    expect(
      screen.getByText(/showing 1 to 20 of 25 entries/i),
    ).toBeInTheDocument()
    expect(screen.getByText(/page 1 of 2/i)).toBeInTheDocument()
    expect(screen.getByTestId("paginationPrevious")).toBeDisabled()
    expect(screen.getByTestId("paginationNext")).not.toBeDisabled()

    await userEvent.click(screen.getByTestId("paginationPrevious"))
    mocks.useGetMoviesQuery.mockReturnValue({
      data: MOVIE_LIST_WITH_MULTIPLE_PAGES,
      isLoading: false,
      isSuccess: true,
      isFetching: false,
      isError: false,
      error: {},
    })
    rerender(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    expect(
      screen.getByText(/showing 1 to 20 of 25 entries/i),
    ).toBeInTheDocument()
    expect(screen.getByText(/page 1 of 2/i)).toBeInTheDocument()
    expect(screen.getByTestId("paginationPrevious")).toBeDisabled()
    expect(screen.getByTestId("paginationNext")).not.toBeDisabled()

    await userEvent.click(screen.getByTestId("paginationNext"))
    mocks.useGetMoviesQuery.mockReturnValue({
      data: MOVIE_LIST_WITH_MULTIPLE_PAGES_2,
      isLoading: false,
      isSuccess: true,
      isFetching: false,
      isError: false,
      error: {},
    })
    rerender(
      <Provider store={store}>
        <BrowserRouter>
          <SearchPage />
        </BrowserRouter>
      </Provider>,
    )
    expect(
      screen.getByText(/showing 21 to 25 of 25 entries/i),
    ).toBeInTheDocument()
    expect(screen.getByText(/page 2 of 2/i)).toBeInTheDocument()
    expect(screen.getByTestId("paginationPrevious")).not.toBeDisabled()
    expect(screen.getByTestId("paginationNext")).toBeDisabled()
    await userEvent.clear(screen.getByTestId("searchInput"))
  })
})
