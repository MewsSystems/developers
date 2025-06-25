import { act, renderHook } from "@testing-library/react"
import type { ReactNode } from "react"
import { MemoryRouter } from "react-router"
import { describe, expect, it } from "vitest"
import { useSearchState } from "../useSearchState"

const createWrapper = (initialEntries: string[] = ["/"]) => {
  const Wrapper = ({ children }: { children: ReactNode }) => {
    return <MemoryRouter initialEntries={initialEntries}>{children}</MemoryRouter>
  }
  return Wrapper
}

describe("useSearchState", () => {
  describe("initial state", () => {
    it("should initialize with empty values when no search params", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(),
      })

      expect(result.current.searchQuery).toBe("")
      expect(result.current.searchPage).toBe(1)
      expect(result.current.popularPage).toBe(1)
    })

    it("should initialize with values from search params", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(["/?q=inception&search_page=2&popular_page=3"]),
      })

      expect(result.current.searchQuery).toBe("inception")
      expect(result.current.searchPage).toBe(2)
      expect(result.current.popularPage).toBe(3)
    })

    it("should handle malformed numeric params", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(["/?q=test&search_page=invalid&popular_page=invalid"]),
      })

      expect(result.current.searchQuery).toBe("test")
      expect(result.current.searchPage).toBe(1)
      expect(result.current.popularPage).toBe(1)
    })
  })

  describe("setSearchQuery", () => {
    it("should update search query", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(),
      })

      act(() => {
        result.current.setSearchQuery("batman")
      })

      expect(result.current.searchQuery).toBe("batman")
    })

    it("should reset search page to 1 when query changes", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(["/?q=initial&search_page=5"]),
      })

      expect(result.current.searchPage).toBe(5)

      act(() => {
        result.current.setSearchQuery("new query")
      })

      expect(result.current.searchQuery).toBe("new query")
      expect(result.current.searchPage).toBe(1)
    })

    it("should not reset search page when query is the same (ignoring whitespace)", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(["/?q=batman&search_page=3"]),
      })

      expect(result.current.searchPage).toBe(3)

      act(() => {
        result.current.setSearchQuery("  batman  ")
      })

      expect(result.current.searchQuery).toBe("  batman  ")
      expect(result.current.searchPage).toBe(3)
    })
  })

  describe("setSearchPage", () => {
    it("should update search page", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(),
      })

      act(() => {
        result.current.setSearchPage(5)
      })

      expect(result.current.searchPage).toBe(5)
    })
  })

  describe("setPopularPage", () => {
    it("should update popular page", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(),
      })

      act(() => {
        result.current.setPopularPage(3)
      })

      expect(result.current.popularPage).toBe(3)
    })
  })

  describe("resetSearch", () => {
    it("should reset search query and search page", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(["/?q=batman&search_page=5&popular_page=3"]),
      })

      expect(result.current.searchQuery).toBe("batman")
      expect(result.current.searchPage).toBe(5)
      expect(result.current.popularPage).toBe(3)

      act(() => {
        result.current.resetSearch()
      })

      expect(result.current.searchQuery).toBe("")
      expect(result.current.searchPage).toBe(1)
      expect(result.current.popularPage).toBe(3)
    })
  })

  describe("URL synchronization", () => {
    it("should set search params when search query is provided", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(),
      })

      act(() => {
        result.current.setSearchQuery("batman")
      })

      expect(result.current.searchQuery).toBe("batman")
    })

    it("should set popular page param when no search query", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(),
      })

      act(() => {
        result.current.setPopularPage(2)
      })

      expect(result.current.popularPage).toBe(2)
    })
  })

  describe("return interface", () => {
    it("should return all expected properties and functions", () => {
      const { result } = renderHook(() => useSearchState(), {
        wrapper: createWrapper(),
      })

      expect(typeof result.current.searchQuery).toBe("string")
      expect(typeof result.current.searchPage).toBe("number")
      expect(typeof result.current.popularPage).toBe("number")
      expect(typeof result.current.setSearchQuery).toBe("function")
      expect(typeof result.current.setSearchPage).toBe("function")
      expect(typeof result.current.setPopularPage).toBe("function")
      expect(typeof result.current.resetSearch).toBe("function")
    })
  })
})
