import { render, screen } from "@testing-library/react"
import { BrowserRouter } from "react-router"
import { ThemeProvider } from "styled-components"
import { describe, expect, it } from "vitest"
import { theme } from "@/styles/theme"
import { mockMovieBase } from "@/test/mocks/movieFixtures"
import type { Movie } from "@/types/movie"
import { MovieCard } from "./MovieCard"

const renderMovieCard = (movie: Movie) => {
  return render(
    <BrowserRouter>
      <ThemeProvider theme={theme}>
        <MovieCard movie={movie} />
      </ThemeProvider>
    </BrowserRouter>
  )
}

describe("MovieCard", () => {
  it("renders movie information correctly", () => {
    renderMovieCard(mockMovieBase)

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.getByText(/Plagued by a violent recurring nightmare/)).toBeInTheDocument()
    expect(screen.getByText("7.2")).toBeInTheDocument()
    expect(screen.getByText("2025")).toBeInTheDocument()
  })

  it("renders movie poster when available", () => {
    renderMovieCard(mockMovieBase)

    const poster = screen.getByAltText("Final Destination Bloodlines")
    expect(poster).toBeInTheDocument()
    expect(poster).toHaveAttribute(
      "src",
      "https://image.tmdb.org/t/p/w500/6WxhEvFsauuACfv8HyoVX6mZKFj.jpg"
    )
  })

  it("renders poster placeholder when poster is not available", () => {
    const movieWithoutPoster = { ...mockMovieBase, poster_path: null }
    const { container } = renderMovieCard(movieWithoutPoster)

    const filmIcon = container.querySelector("svg.lucide-film")
    expect(filmIcon).toBeInTheDocument()
  })

  it("creates correct link to movie detail page", () => {
    renderMovieCard(mockMovieBase)

    const link = screen.getByRole("link")
    expect(link).toHaveAttribute("href", "/movie/574475")
  })

  it("displays formatted rating", () => {
    renderMovieCard(mockMovieBase)

    expect(screen.getByText("7.2")).toBeInTheDocument()
  })

  it("displays release year", () => {
    renderMovieCard(mockMovieBase)

    expect(screen.getByText("2025")).toBeInTheDocument()
  })

  it("handles movie without overview", () => {
    const movieWithoutOverview = { ...mockMovieBase, overview: "" }
    renderMovieCard(movieWithoutOverview)

    expect(screen.getByText("Final Destination Bloodlines")).toBeInTheDocument()
    expect(screen.queryByText(/Plagued by a violent/)).not.toBeInTheDocument()
  })
})
