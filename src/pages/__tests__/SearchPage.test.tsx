import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { render, screen } from "@testing-library/react"
import { BrowserRouter } from "react-router"
import { ThemeProvider } from "styled-components"
import { beforeEach, describe, expect, it, vi } from "vitest"
import { theme } from "../../styles/theme"
import { SearchPage } from "../SearchPage"

const createTestQueryClient = () =>
  new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
      },
    },
  })

const TestWrapper = ({ children }: { children: React.ReactNode }) => {
  const queryClient = createTestQueryClient()

  return (
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <ThemeProvider theme={theme}>{children}</ThemeProvider>
      </BrowserRouter>
    </QueryClientProvider>
  )
}

describe("SearchPage", () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it("renders the search page correctly", () => {
    render(
      <TestWrapper>
        <SearchPage />
      </TestWrapper>
    )

    expect(screen.getByRole("heading", { name: /movie search/i })).toBeInTheDocument()

    expect(screen.getByPlaceholderText(/search for movies/i)).toBeInTheDocument()
  })

  it("displays the search input placeholder", () => {
    render(
      <TestWrapper>
        <SearchPage />
      </TestWrapper>
    )

    const searchInput = screen.getByPlaceholderText(/search for movies/i)
    expect(searchInput).toHaveAttribute("placeholder", "Search for movies...")
  })
})
