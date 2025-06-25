import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import { renderHook, waitFor } from "@testing-library/react"
import type { ReactNode } from "react"
import { describe, expect, it } from "vitest"
import { useMovieDetails, useMovieSearch, usePopularMovies } from "../useMovies"

const createTestQueryClient = () =>
  new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
      },
    },
  })

const createWrapper = () => {
  const queryClient = createTestQueryClient()

  const Wrapper = ({ children }: { children: ReactNode }) => {
    return <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>
  }

  return Wrapper
}

describe("useMovies hooks", () => {
  describe("usePopularMovies", () => {
    it("should fetch popular movies", async () => {
      const { result } = renderHook(() => usePopularMovies(), {
        wrapper: createWrapper(),
      })

      expect(result.current.isLoading).toBe(true)

      await waitFor(() => {
        expect(result.current.isSuccess).toBe(true)
      })

      expect(result.current.data).toBeDefined()
      expect(result.current.data?.results).toHaveLength(4)
      expect(result.current.data?.results[0].title).toBe("Final Destination Bloodlines")
    })

    it("should fetch popular movies for specific page", async () => {
      const { result } = renderHook(() => usePopularMovies(2), {
        wrapper: createWrapper(),
      })

      await waitFor(() => {
        expect(result.current.isSuccess).toBe(true)
      })

      expect(result.current.data).toBeDefined()
    })
  })

  describe("useMovieSearch", () => {
    it("should search movies with query", async () => {
      const { result } = renderHook(() => useMovieSearch("Final Destination"), {
        wrapper: createWrapper(),
      })

      expect(result.current.isLoading).toBe(true)

      await waitFor(() => {
        expect(result.current.isSuccess).toBe(true)
      })

      expect(result.current.data).toBeDefined()
      expect(result.current.data?.results).toHaveLength(1)
      expect(result.current.data?.results[0].title).toBe("Final Destination Bloodlines")
    })

    it("should not execute when query is empty", () => {
      const { result } = renderHook(() => useMovieSearch(""), {
        wrapper: createWrapper(),
      })

      expect(result.current.isLoading).toBe(false)
      expect(result.current.data).toBeUndefined()
    })

    it("should not execute when query is only whitespace", () => {
      const { result } = renderHook(() => useMovieSearch("   "), {
        wrapper: createWrapper(),
      })

      expect(result.current.isLoading).toBe(false)
      expect(result.current.data).toBeUndefined()
    })
  })

  describe("useMovieDetails", () => {
    it("should fetch movie details", async () => {
      const { result } = renderHook(() => useMovieDetails(574475), {
        wrapper: createWrapper(),
      })

      expect(result.current.isLoading).toBe(true)

      await waitFor(() => {
        expect(result.current.isSuccess).toBe(true)
      })

      expect(result.current.data).toBeDefined()
      expect(result.current.data?.id).toBe(574475)
      expect(result.current.data?.title).toBe("Final Destination Bloodlines")
    })

    it("should not execute when id is 0", () => {
      const { result } = renderHook(() => useMovieDetails(0), {
        wrapper: createWrapper(),
      })

      expect(result.current.isLoading).toBe(false)
      expect(result.current.data).toBeUndefined()
    })

    it("should not execute when id is invalid", () => {
      const { result } = renderHook(() => useMovieDetails(-1), {
        wrapper: createWrapper(),
      })

      expect(result.current.isLoading).toBe(false)
      expect(result.current.data).toBeUndefined()
    })
  })
})
