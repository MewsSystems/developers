import { render, screen, waitFor } from "@testing-library/react"
import { MemoryRouter } from "react-router"
import { describe, expect, it, vi } from "vitest"
import App from "./App"
import { mockMovieDetails, mockPopularMoviesResponse } from "./test/mocks/movieFixtures"

vi.mock("./hooks/useMovies", () => ({
  usePopularMovies: vi.fn(() => ({
    data: mockPopularMoviesResponse,
    isLoading: false,
    error: null,
  })),
  useMovieDetails: vi.fn(() => ({
    data: mockMovieDetails,
    isLoading: false,
    error: null,
  })),
  useMovieSearch: vi.fn(() => ({
    data: null,
    isLoading: false,
    error: null,
  })),
}))

const TestWrapper = ({ initialEntries = ["/"] }: { initialEntries?: string[] }) => (
  <MemoryRouter initialEntries={initialEntries}>
    <App />
  </MemoryRouter>
)

describe("App", () => {
  it("renders search page by default", async () => {
    render(<TestWrapper />)

    await waitFor(() => {
      expect(screen.getByText("TMDB Movie Search")).toBeInTheDocument()
    })
    expect(screen.getByPlaceholderText("Search for movies...")).toBeInTheDocument()
  })

  it("renders movie detail page when navigating to /movie/:id", async () => {
    render(<TestWrapper initialEntries={["/movie/550"]} />)

    await waitFor(() => {
      expect(screen.getByText("Back to Search")).toBeInTheDocument()
    })
  })

  it("handles unknown routes gracefully", async () => {
    render(<TestWrapper initialEntries={["/unknown-route"]} />)

    await waitFor(() => {
      expect(screen.getByText("404")).toBeInTheDocument()
    })
    expect(screen.getByText("Page Not Found")).toBeInTheDocument()
    expect(screen.getByText("Back to Movie Search")).toBeInTheDocument()
  })
})
