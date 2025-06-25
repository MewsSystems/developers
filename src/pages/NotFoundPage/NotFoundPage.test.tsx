import { render, screen } from "@testing-library/react"
import { BrowserRouter } from "react-router"
import { ThemeProvider } from "styled-components"
import { describe, expect, it } from "vitest"
import { theme } from "@/styles/theme"
import { NotFoundPage } from "./NotFoundPage"

const renderNotFoundPage = () => {
  return render(
    <BrowserRouter>
      <ThemeProvider theme={theme}>
        <NotFoundPage />
      </ThemeProvider>
    </BrowserRouter>
  )
}

describe("NotFoundPage", () => {
  it("renders 404 error message", () => {
    renderNotFoundPage()

    expect(screen.getByText("404")).toBeInTheDocument()
    expect(screen.getByText("Page Not Found")).toBeInTheDocument()
  })

  it("displays helpful error message", () => {
    renderNotFoundPage()

    expect(
      screen.getByText(
        /Oops! The page you're looking for doesn't exist. It might have been moved, deleted, or you entered the wrong URL./
      )
    ).toBeInTheDocument()
  })

  it("displays movie icon", () => {
    const { container } = renderNotFoundPage()

    const filmIcon = container.querySelector("svg.lucide-film")
    expect(filmIcon).toBeInTheDocument()
  })

  it("has a link back to home page", () => {
    renderNotFoundPage()

    const backLink = screen.getByRole("link", { name: /back to movie search/i })
    expect(backLink).toBeInTheDocument()
    expect(backLink).toHaveAttribute("href", "/")
  })
})
