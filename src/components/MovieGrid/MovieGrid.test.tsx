import { render, screen } from "@testing-library/react"
import { BrowserRouter } from "react-router"
import { ThemeProvider } from "styled-components"
import { describe, expect, it } from "vitest"
import { theme } from "@/styles/theme"
import { mockMovieBase, mockMovies } from "@/test/mocks/movieFixtures"
import { MovieGrid } from "./MovieGrid"

const renderMovieGrid = (props = {}) => {
  return render(
    <BrowserRouter>
      <ThemeProvider theme={theme}>
        <MovieGrid {...props} />
      </ThemeProvider>
    </BrowserRouter>
  )
}

describe("MovieGrid", () => {
  it("renders loading skeletons when isLoading is true", () => {
    const { container } = renderMovieGrid({ isLoading: true })

    const grid = container.firstChild
    expect(grid).toBeInTheDocument()
    expect(grid?.childNodes).toHaveLength(8)
  })

  it("renders empty grid when no movies provided", () => {
    const { container } = renderMovieGrid({ movies: [] })

    expect(container.firstChild).toBeNull()
  })

  it("renders movies correctly", () => {
    renderMovieGrid({ movies: mockMovies })

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.getByText("Lilo & Stitch")).toBeInTheDocument()
    expect(screen.getByText("How to Train Your Dragon")).toBeInTheDocument()
    expect(screen.getByText("STRAW")).toBeInTheDocument()
  })

  it("passes correct movie data to each MovieCard", () => {
    renderMovieGrid({ movies: mockMovies })

    expect(screen.getByText("7.2")).toBeInTheDocument()
    expect(screen.getByText("7.1")).toBeInTheDocument()
    expect(screen.getByText("8.1")).toBeInTheDocument()
    expect(screen.getByText("8.0")).toBeInTheDocument()
  })

  it("renders custom skeleton count when provided", () => {
    const { container } = renderMovieGrid({ isLoading: true, skeletonCount: 4 })

    const grid = container.firstChild
    expect(grid?.childNodes).toHaveLength(4)
  })

  it("creates correct links for each movie", () => {
    renderMovieGrid({ movies: mockMovies })

    const links = screen.getAllByRole("link")
    expect(links[0]).toHaveAttribute("href", "/movie/574475")
    expect(links[1]).toHaveAttribute("href", "/movie/552524")
    expect(links[2]).toHaveAttribute("href", "/movie/1087192")
    expect(links[3]).toHaveAttribute("href", "/movie/1426776")
  })

  it("handles single movie correctly", () => {
    const singleMovie = [mockMovieBase]
    renderMovieGrid({ movies: singleMovie })

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.queryByText("Lilo & Stitch")).not.toBeInTheDocument()
    expect(screen.queryByText("How to Train Your Dragon")).not.toBeInTheDocument()
    expect(screen.queryByText("STRAW")).not.toBeInTheDocument()

    const links = screen.getAllByRole("link")
    expect(links).toHaveLength(1)
  })

  it("uses movie id as key for each MovieCard", () => {
    renderMovieGrid({ movies: mockMovies })

    const movieTitles = screen.getAllByRole("heading", { level: 3 })
    expect(movieTitles).toHaveLength(4)

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.getByText("Lilo & Stitch")).toBeInTheDocument()
    expect(screen.getByText("How to Train Your Dragon")).toBeInTheDocument()
    expect(screen.getByText("STRAW")).toBeInTheDocument()
  })
})
