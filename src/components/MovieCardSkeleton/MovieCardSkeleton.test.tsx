import { render } from "@testing-library/react"
import { ThemeProvider } from "styled-components"
import { describe, expect, it } from "vitest"
import { theme } from "@/styles/theme"
import { MovieCardSkeleton } from "./MovieCardSkeleton"

const renderMovieCardSkeleton = () => {
  return render(
    <ThemeProvider theme={theme}>
      <MovieCardSkeleton />
    </ThemeProvider>
  )
}

describe("MovieCardSkeleton", () => {
  it("renders without crashing", () => {
    const { container } = renderMovieCardSkeleton()
    expect(container.firstChild).toBeInTheDocument()
  })

  it("renders the skeleton card container", () => {
    const { container } = renderMovieCardSkeleton()
    const skeletonCard = container.firstChild
    expect(skeletonCard).toBeInTheDocument()
  })

  it("renders all skeleton elements", () => {
    const { container } = renderMovieCardSkeleton()

    const skeletonElements = container.querySelectorAll("div")
    expect(skeletonElements.length).toBeGreaterThan(5)
  })
})
