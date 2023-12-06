import { render, screen } from "@testing-library/react"
import { Provider } from "react-redux"
import { BrowserRouter } from "react-router-dom"
import { store } from "@/app/store"
import ErrorPage from "./ErrorPage"

const mocks = vi.hoisted(() => {
  return {
    isRouteErrorResponse: vi.fn(),
    useRouteError: vi.fn(),
  }
})

vi.mock("react-router-dom", async (importOriginal) => {
  const mod = await importOriginal<typeof import("react-router-dom")>()
  return {
    ...mod,
    useRouteError: mocks.useRouteError,
    isRouteErrorResponse: mocks.isRouteErrorResponse,
  }
})

describe("ErrorPage", () => {
  afterEach(() => {
    vi.clearAllMocks()
  })

  test("is route error response and error statusText exist", () => {
    mocks.isRouteErrorResponse.mockReturnValue(true)
    mocks.useRouteError.mockReturnValue({ statusText: "test error 1" })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <ErrorPage />
        </BrowserRouter>
      </Provider>,
    )

    expect(screen.getByText(/test error 1/i)).toBeInTheDocument()
  })

  test("is route error response and error statusText doesn't exist", () => {
    mocks.isRouteErrorResponse.mockReturnValue(true)
    mocks.useRouteError.mockReturnValue({ data: { message: "test error 2" } })

    render(
      <Provider store={store}>
        <BrowserRouter>
          <ErrorPage />
        </BrowserRouter>
      </Provider>,
    )

    expect(screen.getByText(/test error 2/i)).toBeInTheDocument()
  })

  test("is not route error response", () => {
    mocks.isRouteErrorResponse.mockReturnValue(false)

    render(
      <Provider store={store}>
        <BrowserRouter>
          <ErrorPage />
        </BrowserRouter>
      </Provider>,
    )

    expect(screen.getByText(/unknown error/i)).toBeInTheDocument()
  })
})
