import { render } from "@testing-library/react"
import { ThemeProvider } from "styled-components"
import { describe, expect, it } from "vitest"
import { theme } from "@/styles/theme"
import { PageLoader } from "./PageLoader"

const renderPageLoader = () => {
  return render(
    <ThemeProvider theme={theme}>
      <PageLoader />
    </ThemeProvider>
  )
}

describe("PageLoader", () => {
  it("renders the film icon", () => {
    const { container } = renderPageLoader()

    const filmIcon = container.querySelector("svg.lucide-film")
    expect(filmIcon).toBeInTheDocument()
  })
})
