import { fireEvent, render, screen } from "@testing-library/react"
import { ThemeProvider } from "styled-components"
import { beforeEach, describe, expect, it, vi } from "vitest"
import type { UsePaginationReturn } from "../../hooks/usePagination"
import { theme } from "../../styles/theme"
import { Pagination } from "../Pagination"

const mockPaginationProps: UsePaginationReturn = {
  currentPage: 5,
  totalPages: 10,
  visiblePages: [3, 4, 5, 6, 7],
  canGoPrevious: true,
  canGoNext: true,
  goToPage: vi.fn(),
  goToPrevious: vi.fn(),
  goToNext: vi.fn(),
}

const renderPagination = (props: Partial<UsePaginationReturn> = {}) => {
  return render(
    <ThemeProvider theme={theme}>
      <Pagination {...mockPaginationProps} {...props} />
    </ThemeProvider>
  )
}

describe("Pagination", () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it("renders pagination controls correctly", () => {
    renderPagination()

    expect(screen.getByLabelText("Go to previous page")).toBeInTheDocument()
    expect(screen.getByLabelText("Go to next page")).toBeInTheDocument()
    expect(screen.getByText("Page 5 of 10")).toBeInTheDocument()
  })

  it("renders visible page numbers", () => {
    renderPagination()

    expect(screen.getByLabelText("Go to page 3")).toBeInTheDocument()
    expect(screen.getByLabelText("Go to page 4")).toBeInTheDocument()
    expect(screen.getByLabelText("Go to page 5")).toBeInTheDocument()
    expect(screen.getByLabelText("Go to page 6")).toBeInTheDocument()
    expect(screen.getByLabelText("Go to page 7")).toBeInTheDocument()
  })

  it("highlights current page", () => {
    renderPagination()

    const currentPageButton = screen.getByLabelText("Go to page 5")
    expect(currentPageButton).toHaveAttribute("aria-current", "page")
  })

  it("calls goToPage when page number is clicked", () => {
    renderPagination()

    fireEvent.click(screen.getByLabelText("Go to page 3"))
    expect(mockPaginationProps.goToPage).toHaveBeenCalledWith(3)
  })

  it("calls navigation functions correctly", () => {
    renderPagination()

    fireEvent.click(screen.getByLabelText("Go to previous page"))
    expect(mockPaginationProps.goToPrevious).toHaveBeenCalled()

    fireEvent.click(screen.getByLabelText("Go to next page"))
    expect(mockPaginationProps.goToNext).toHaveBeenCalled()
  })

  it("disables navigation buttons when appropriate", () => {
    renderPagination({
      currentPage: 1,
      canGoPrevious: false,
      canGoNext: false,
    })

    expect(screen.getByLabelText("Go to previous page")).toBeDisabled()
    expect(screen.getByLabelText("Go to next page")).toBeDisabled()
  })

  it("shows ellipsis when there are many pages", () => {
    renderPagination({
      currentPage: 10,
      totalPages: 50,
      visiblePages: [8, 9, 10, 11, 12],
    })

    const ellipses = screen.getAllByText("…")
    expect(ellipses.length).toBeGreaterThan(0)
  })

  it("shows first and last page when not in visible range", () => {
    renderPagination({
      currentPage: 25,
      totalPages: 50,
      visiblePages: [23, 24, 25, 26, 27],
    })

    expect(screen.getByLabelText("Go to page 1")).toBeInTheDocument()
    expect(screen.getByLabelText("Go to page 50")).toBeInTheDocument()
  })

  it("does not render when totalPages is 1 or less", () => {
    const { container } = renderPagination({
      totalPages: 1,
      currentPage: 1,
      visiblePages: [1],
    })

    expect(container).toBeEmptyDOMElement()
  })
})
