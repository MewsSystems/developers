import { fireEvent, render, screen } from "@testing-library/react"
import { ThemeProvider } from "styled-components"
import { beforeEach, describe, expect, it, vi } from "vitest"
import { theme } from "@/styles/theme"
import { SearchInput } from "./SearchInput"

const mockOnChange = vi.fn()

const renderSearchInput = (props = {}) => {
  const defaultProps = {
    value: "",
    onChange: mockOnChange,
    placeholder: "Search for movies...",
  }

  return render(
    <ThemeProvider theme={theme}>
      <SearchInput {...defaultProps} {...props} />
    </ThemeProvider>
  )
}

describe("SearchInput", () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it("renders with correct placeholder", () => {
    renderSearchInput()

    expect(screen.getByPlaceholderText("Search for movies...")).toBeInTheDocument()
  })

  it("displays the current value", () => {
    renderSearchInput({ value: "Final Destination" })

    expect(screen.getByDisplayValue("Final Destination")).toBeInTheDocument()
  })

  it("calls onChange when typing", () => {
    renderSearchInput()

    const input = screen.getByPlaceholderText("Search for movies...")
    fireEvent.change(input, { target: { value: "Test Movie" } })

    expect(mockOnChange).toHaveBeenCalledWith("Test Movie")
  })

  it("renders search icon", () => {
    const { container } = renderSearchInput()

    const searchIcon = container.querySelector("svg.lucide-search")
    expect(searchIcon).toBeInTheDocument()
  })

  it("uses custom placeholder when provided", () => {
    renderSearchInput({ placeholder: "Custom placeholder" })

    expect(screen.getByPlaceholderText("Custom placeholder")).toBeInTheDocument()
  })

  it("handles empty value", () => {
    renderSearchInput({ value: "" })

    const input = screen.getByPlaceholderText("Search for movies...")
    expect(input).toHaveValue("")
  })
})
