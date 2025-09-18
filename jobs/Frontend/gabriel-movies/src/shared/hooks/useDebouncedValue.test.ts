import { describe, it, expect, vi } from "vitest";
import { renderHook, act } from "@testing-library/react";
import { useDebouncedValue } from "./useDebouncedValue";

describe("useDebouncedValue", () => {
  it("returns the last value after the delay", async () => {
    vi.useFakeTimers()
    const { result, rerender } = renderHook(({ v }) => useDebouncedValue(v), {
      initialProps: { v: "" }
    });

    rerender({ v: "A" });
    rerender({ v: "Avatar" });

    await act(async () => {
      await vi.advanceTimersByTimeAsync(400);
    });

    expect(result.current).toBe("Avatar");
    vi.useRealTimers()
  });
});
