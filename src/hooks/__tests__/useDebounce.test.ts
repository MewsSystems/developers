import { act, renderHook } from "@testing-library/react"
import { afterEach, beforeEach, describe, expect, it, vi } from "vitest"
import { useDebounce } from "../useDebounce"

describe("useDebounce", () => {
  beforeEach(() => {
    vi.useFakeTimers()
  })

  afterEach(() => {
    vi.useRealTimers()
  })

  it("should return initial value immediately", () => {
    const { result } = renderHook(() => useDebounce("initial", 500))

    expect(result.current).toBe("initial")
  })

  it("should debounce value updates", () => {
    const { result, rerender } = renderHook(({ value, delay }) => useDebounce(value, delay), {
      initialProps: { value: "initial", delay: 500 },
    })

    expect(result.current).toBe("initial")

    rerender({ value: "updated", delay: 500 })

    expect(result.current).toBe("initial")

    act(() => {
      vi.advanceTimersByTime(400)
    })

    expect(result.current).toBe("initial")

    act(() => {
      vi.advanceTimersByTime(100)
    })

    expect(result.current).toBe("updated")
  })

  it("should cancel previous timeout when value changes quickly", () => {
    const { result, rerender } = renderHook(({ value, delay }) => useDebounce(value, delay), {
      initialProps: { value: "initial", delay: 500 },
    })

    rerender({ value: "first", delay: 500 })

    act(() => {
      vi.advanceTimersByTime(300)
    })

    rerender({ value: "second", delay: 500 })

    act(() => {
      vi.advanceTimersByTime(300)
    })

    expect(result.current).toBe("initial")

    act(() => {
      vi.advanceTimersByTime(200)
    })

    expect(result.current).toBe("second")
  })

  it("should work with different data types", () => {
    const { result, rerender } = renderHook(({ value, delay }) => useDebounce(value, delay), {
      initialProps: { value: 0, delay: 500 },
    })

    expect(result.current).toBe(0)

    rerender({ value: 42, delay: 500 })

    act(() => {
      vi.advanceTimersByTime(500)
    })

    expect(result.current).toBe(42)
  })

  it("should handle delay changes", () => {
    const { result, rerender } = renderHook(({ value, delay }) => useDebounce(value, delay), {
      initialProps: { value: "initial", delay: 500 },
    })

    rerender({ value: "updated", delay: 1000 })

    act(() => {
      vi.advanceTimersByTime(500)
    })

    expect(result.current).toBe("initial")

    act(() => {
      vi.advanceTimersByTime(500)
    })

    expect(result.current).toBe("updated")
  })
})
