import { render } from "@testing-library/react"
import { ThemeProvider } from "styled-components"
import { describe, expect, it } from "vitest"
import { theme } from "@/styles/theme"
import { MovieDetailSkeleton } from "./MovieDetailSkeleton"

const renderMovieDetailSkeleton = () => {
  return render(
    <ThemeProvider theme={theme}>
      <MovieDetailSkeleton />
    </ThemeProvider>
  )
}

describe("MovieDetailSkeleton", () => {
  it("renders without crashing", () => {
    const { container } = renderMovieDetailSkeleton()
    expect(container.firstChild).toBeInTheDocument()
  })

  it("renders the main container", () => {
    const { container } = renderMovieDetailSkeleton()
    const container_element = container.firstChild
    expect(container_element).toBeInTheDocument()
  })

  it("renders all skeleton sections", () => {
    const { container } = renderMovieDetailSkeleton()

    const skeletonElements = container.querySelectorAll("div")

    expect(skeletonElements.length).toBeGreaterThan(15)
  })

  it("has the correct header structure", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    expect(mainContainer.children.length).toBeGreaterThan(0)

    const header = mainContainer.children[0] as HTMLElement
    expect(header).toBeInTheDocument()
    expect(header.children.length).toBe(2)
  })

  it("renders poster skeleton", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    const header = mainContainer.children[0] as HTMLElement
    const posterContainer = header.children[0] as HTMLElement

    expect(posterContainer).toBeInTheDocument()
    expect(posterContainer.children.length).toBe(1)
  })

  it("renders info section skeletons", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    const header = mainContainer.children[0] as HTMLElement
    const infoSection = header.children[1] as HTMLElement

    expect(infoSection).toBeInTheDocument()
    expect(infoSection.children.length).toBeGreaterThan(3)
  })

  it("renders title skeleton", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    const header = mainContainer.children[0] as HTMLElement
    const infoSection = header.children[1] as HTMLElement
    const titleSkeleton = infoSection.children[0] as HTMLElement

    expect(titleSkeleton).toBeInTheDocument()
  })

  it("renders meta info skeletons", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    const header = mainContainer.children[0] as HTMLElement
    const infoSection = header.children[1] as HTMLElement
    const metaInfo = infoSection.children[1] as HTMLElement

    expect(metaInfo).toBeInTheDocument()
    expect(metaInfo.children.length).toBe(4)
  })

  it("renders genre skeletons", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    const header = mainContainer.children[0] as HTMLElement
    const infoSection = header.children[1] as HTMLElement
    const genreList = infoSection.children[2] as HTMLElement

    expect(genreList).toBeInTheDocument()
    expect(genreList.children.length).toBe(3)
  })

  it("renders overview skeletons", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    const header = mainContainer.children[0] as HTMLElement
    const infoSection = header.children[1] as HTMLElement
    const overviewSection = infoSection.children[3] as HTMLElement

    expect(overviewSection).toBeInTheDocument()
    expect(overviewSection.children.length).toBe(5)
  })

  it("renders production section", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    expect(mainContainer.children.length).toBe(2)

    const productionSection = mainContainer.children[1] as HTMLElement
    expect(productionSection).toBeInTheDocument()
    expect(productionSection.children.length).toBe(2)
  })

  it("renders production list skeletons", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    const productionSection = mainContainer.children[1] as HTMLElement
    const productionList = productionSection.children[1] as HTMLElement

    expect(productionList).toBeInTheDocument()
    expect(productionList.children.length).toBe(3)
  })

  it("has accessible skeleton elements", () => {
    const { container } = renderMovieDetailSkeleton()

    const buttons = container.querySelectorAll("button")
    const links = container.querySelectorAll("a")
    const inputs = container.querySelectorAll("input")

    expect(buttons.length).toBe(0)
    expect(links.length).toBe(0)
    expect(inputs.length).toBe(0)
  })

  it("maintains responsive layout structure", () => {
    const { container } = renderMovieDetailSkeleton()

    const mainContainer = container.firstChild as HTMLElement
    expect(mainContainer).toBeInTheDocument()

    const header = mainContainer.children[0] as HTMLElement
    expect(header.children.length).toBe(2)
  })
})
