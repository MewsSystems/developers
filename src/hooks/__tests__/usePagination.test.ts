import { act, renderHook } from "@testing-library/react"
import { describe, expect, it } from "vitest"
import { usePagination } from "../usePagination"

describe("usePagination", () => {
  it("initializes with default values", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 10,
      })
    )

    expect(result.current.currentPage).toBe(1)
    expect(result.current.totalPages).toBe(10)
    expect(result.current.canGoPrevious).toBe(false)
    expect(result.current.canGoNext).toBe(true)
  })

  it("initializes with custom initial page", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 10,
        initialPage: 5,
      })
    )

    expect(result.current.currentPage).toBe(5)
    expect(result.current.canGoPrevious).toBe(true)
    expect(result.current.canGoNext).toBe(true)
  })

  it("calculates visible pages correctly for small total pages", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 3,
        maxVisiblePages: 5,
      })
    )

    expect(result.current.visiblePages).toEqual([1, 2, 3])
  })

  it("calculates visible pages correctly for large total pages", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 20,
        initialPage: 10,
        maxVisiblePages: 5,
      })
    )

    expect(result.current.visiblePages).toEqual([8, 9, 10, 11, 12])
  })

  it("adjusts visible pages when near the beginning", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 20,
        initialPage: 2,
        maxVisiblePages: 5,
      })
    )

    expect(result.current.visiblePages).toEqual([1, 2, 3, 4, 5])
  })

  it("adjusts visible pages when near the end", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 20,
        initialPage: 19,
        maxVisiblePages: 5,
      })
    )

    expect(result.current.visiblePages).toEqual([16, 17, 18, 19, 20])
  })

  it("goToPage works correctly", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 10,
        initialPage: 1,
      })
    )

    act(() => {
      result.current.goToPage(5)
    })

    expect(result.current.currentPage).toBe(5)
  })

  it("goToPage ignores invalid page numbers", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 10,
        initialPage: 5,
      })
    )

    act(() => {
      result.current.goToPage(0)
    })
    expect(result.current.currentPage).toBe(5)

    act(() => {
      result.current.goToPage(11)
    })
    expect(result.current.currentPage).toBe(5)
  })

  it("navigation methods work correctly", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 10,
        initialPage: 5,
      })
    )

    act(() => {
      result.current.goToPrevious()
    })
    expect(result.current.currentPage).toBe(4)

    act(() => {
      result.current.goToNext()
    })
    expect(result.current.currentPage).toBe(5)
  })

  it("canGoPrevious and canGoNext work correctly", () => {
    const { result } = renderHook(() =>
      usePagination({
        totalPages: 3,
        initialPage: 1,
      })
    )

    expect(result.current.canGoPrevious).toBe(false)
    expect(result.current.canGoNext).toBe(true)

    act(() => {
      result.current.goToPage(2)
    })

    expect(result.current.canGoPrevious).toBe(true)
    expect(result.current.canGoNext).toBe(true)

    act(() => {
      result.current.goToPage(3)
    })

    expect(result.current.canGoPrevious).toBe(true)
    expect(result.current.canGoNext).toBe(false)
  })
})
