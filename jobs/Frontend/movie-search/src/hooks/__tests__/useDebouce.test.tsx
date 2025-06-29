import { renderHook, act } from "@testing-library/react";
import { vi, describe, it, expect } from "vitest";
import { useDebounce } from "../useDebounce";

vi.useFakeTimers();

describe("useDebounce", () => {
  it("should update debounced value after the specified delay", () => {
    const { result, rerender } = renderHook(
      ({ value, delay }) => useDebounce(value, delay),
      { initialProps: { value: "a", delay: 500 } }
    );

    expect(result.current).toBe("a");

    rerender({ value: "b", delay: 500 });
    expect(result.current).toBe("a");

    act(() => {
      vi.advanceTimersByTime(500);
    });

    expect(result.current).toBe("b");
  });

  it("should reset timer if value changes before delay", () => {
    const { result, rerender } = renderHook(
      ({ value, delay }) => useDebounce(value, delay),
      { initialProps: { value: 0, delay: 1000 } }
    );

    rerender({ value: 1, delay: 1000 });
    act(() => vi.advanceTimersByTime(500));
    rerender({ value: 2, delay: 1000 });
    act(() => vi.advanceTimersByTime(500));
    expect(result.current).toBe(0);

    act(() => vi.advanceTimersByTime(500));
    expect(result.current).toBe(2);
  });
});
