import { QueryClient, QueryClientProvider, type UseQueryResult } from "@tanstack/react-query"
import { render, screen } from "@testing-library/react"
import { MemoryRouter, Route, Routes } from "react-router"
import { ThemeProvider } from "styled-components"
import { beforeEach, describe, expect, it, vi } from "vitest"
import { useMovieDetails } from "../../hooks/useMovies"
import { theme } from "../../styles/theme"
import { mockMovieDetails } from "../../test/mocks/movieFixtures"
import type { MovieDetails } from "../../types/movie"
import { MovieDetailPage } from "../MovieDetailPage"

vi.mock("../../hooks/useMovies")

const createTestQueryClient = () =>
  new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
      },
    },
  })

const TestWrapper = ({
  children,
  initialEntries = ["/movie/574475"],
}: {
  children: React.ReactNode
  initialEntries?: string[]
}) => {
  const queryClient = createTestQueryClient()

  return (
    <QueryClientProvider client={queryClient}>
      <MemoryRouter initialEntries={initialEntries}>
        <ThemeProvider theme={theme}>
          <Routes>
            <Route path="/movie/:id" element={children} />
          </Routes>
        </ThemeProvider>
      </MemoryRouter>
    </QueryClientProvider>
  )
}

describe("MovieDetailPage", () => {
  const mockUseMovieDetails = vi.mocked(useMovieDetails)

  beforeEach(() => {
    vi.clearAllMocks()
  })

  it("displays loading spinner while fetching movie details", () => {
    mockUseMovieDetails.mockReturnValue({
      data: undefined,
      isLoading: true,
      error: null,
    } as UseQueryResult<MovieDetails, Error>)

    const { container } = render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    const skeletonElements = container.querySelectorAll("div")
    expect(skeletonElements.length).toBeGreaterThan(0)
  })

  it("displays movie details correctly when data is loaded", async () => {
    mockUseMovieDetails.mockReturnValue({
      data: mockMovieDetails,
      isLoading: false,
      error: null,
    } as UseQueryResult<MovieDetails, Error>)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.getByText(/Plagued by a violent recurring nightmare/)).toBeInTheDocument()
    expect(screen.getByText("7.2")).toBeInTheDocument()
    expect(screen.getByText("2025")).toBeInTheDocument()
    expect(screen.getByText("1h 50m")).toBeInTheDocument()
    expect(screen.getByText("1,109 votes")).toBeInTheDocument()
  })

  it("displays poster when available", () => {
    mockUseMovieDetails.mockReturnValue({
      data: mockMovieDetails,
      isLoading: false,
      error: null,
    } as UseQueryResult<MovieDetails, Error>)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    const poster = screen.getByAltText("Final Destination Bloodlines")
    expect(poster).toBeInTheDocument()
    expect(poster).toHaveAttribute(
      "src",
      "https://image.tmdb.org/t/p/w500/6WxhEvFsauuACfv8HyoVX6mZKFj.jpg"
    )
  })

  it("displays poster placeholder when poster is not available", () => {
    const movieWithoutPoster = { ...mockMovieDetails, poster_path: null }
    mockUseMovieDetails.mockReturnValue({
      data: movieWithoutPoster,
      isLoading: false,
      error: null,
    } as UseQueryResult<MovieDetails, Error>)

    const { container } = render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    const filmIcon = container.querySelector("svg.lucide-film")
    expect(filmIcon).toBeInTheDocument()
  })

  it("displays genres correctly", () => {
    mockUseMovieDetails.mockReturnValue({
      data: mockMovieDetails,
      isLoading: false,
      error: null,
    } as any)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Horror")).toBeInTheDocument()
    expect(screen.getByText("Mystery")).toBeInTheDocument()
  })

  it("displays production companies correctly", () => {
    mockUseMovieDetails.mockReturnValue({
      data: mockMovieDetails,
      isLoading: false,
      error: null,
    } as UseQueryResult<MovieDetails, Error>)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Production Companies")).toBeInTheDocument()
    expect(screen.getByText("New Line Cinema")).toBeInTheDocument()
    expect(screen.getByText("Practical Pictures")).toBeInTheDocument()
  })

  it("handles movie without runtime correctly", () => {
    const movieWithoutRuntime = { ...mockMovieDetails, runtime: undefined }
    mockUseMovieDetails.mockReturnValue({
      data: movieWithoutRuntime,
      isLoading: false,
      error: null,
    } as UseQueryResult<MovieDetails, Error>)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.queryByText(/\d+h \d+m/)).not.toBeInTheDocument()
  })

  it("displays back button with correct link", () => {
    mockUseMovieDetails.mockReturnValue({
      data: mockMovieDetails,
      isLoading: false,
      error: null,
    } as UseQueryResult<MovieDetails, Error>)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    const backButton = screen.getByRole("link", { name: /back to search/i })
    expect(backButton).toBeInTheDocument()
    expect(backButton).toHaveAttribute("href", "/")
  })

  it("displays error message when there is an error", () => {
    mockUseMovieDetails.mockReturnValue({
      data: undefined,
      isLoading: false,
      error: new Error("Network error"),
    } as UseQueryResult<MovieDetails, Error>)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Failed to load movie details. Please try again.")).toBeInTheDocument()
    const backButton = screen.getByRole("link", { name: /back to search/i })
    expect(backButton).toBeInTheDocument()
  })

  it("handles movie without data", () => {
    mockUseMovieDetails.mockReturnValue({
      data: null,
      isLoading: false,
      error: null,
    } as any)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Failed to load movie details. Please try again.")).toBeInTheDocument()
  })

  it("handles movie without genres", () => {
    const movieWithoutGenres = { ...mockMovieDetails, genres: [] }
    mockUseMovieDetails.mockReturnValue({
      data: movieWithoutGenres,
      isLoading: false,
      error: null,
    } as any)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.queryByText("Horror")).not.toBeInTheDocument()
    expect(screen.queryByText("Mystery")).not.toBeInTheDocument()
  })

  it("handles movie without production companies", () => {
    const movieWithoutCompanies = { ...mockMovieDetails, production_companies: [] }
    mockUseMovieDetails.mockReturnValue({
      data: movieWithoutCompanies,
      isLoading: false,
      error: null,
    } as any)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.queryByText("Production Companies")).not.toBeInTheDocument()
  })

  it("handles movie without overview", () => {
    const movieWithoutOverview = { ...mockMovieDetails, overview: "" }
    mockUseMovieDetails.mockReturnValue({
      data: movieWithoutOverview,
      isLoading: false,
      error: null,
    } as any)

    render(
      <TestWrapper>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.queryByText(/Plagued by a violent/)).not.toBeInTheDocument()
  })

  it("parses movie ID from URL correctly", () => {
    mockUseMovieDetails.mockReturnValue({
      data: mockMovieDetails,
      isLoading: false,
      error: null,
    } as any)

    render(
      <TestWrapper initialEntries={["/movie/123456"]}>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(mockUseMovieDetails).toHaveBeenCalledWith(123456)
  })

  it("handles invalid movie ID from URL", () => {
    mockUseMovieDetails.mockReturnValue({
      data: undefined,
      isLoading: false,
      error: new Error("Invalid ID"),
    } as any)

    render(
      <TestWrapper initialEntries={["/movie/invalid"]}>
        <MovieDetailPage />
      </TestWrapper>
    )

    expect(mockUseMovieDetails).toHaveBeenCalledWith(0)
  })
})
