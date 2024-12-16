import React from "react";
import { renderHook, act } from "@testing-library/react";
import { useDebouncedValue } from "./useDebouncedValue";

jest.useFakeTimers();

describe("useDebouncedValue", () => {
  it("returns initial value immediately", () => {
    const { result } = renderHook(() => useDebouncedValue("initial", 300));
    expect(result.current).toBe("initial");
  });

  it("updates value after the delay", () => {
    const { result, rerender } = renderHook(
      ({ value, delay }) => useDebouncedValue(value, delay),
      {
        initialProps: { value: "initial", delay: 300 },
      }
    );

    expect(result.current).toBe("initial");

    rerender({ value: "updated", delay: 300 });

    expect(result.current).toBe("initial");

    act(() => {
      jest.advanceTimersByTime(300);
    });

    expect(result.current).toBe("updated");
  });

  it("resets timer if value changes before delay", () => {
    const { result, rerender } = renderHook(
      ({ value, delay }) => useDebouncedValue(value, delay),
      {
        initialProps: { value: "one", delay: 300 },
      }
    );

    expect(result.current).toBe("one");

    rerender({ value: "two", delay: 300 });
    act(() => {
      jest.advanceTimersByTime(100);
    });

    expect(result.current).toBe("one");

    act(() => {
      jest.advanceTimersByTime(200);
    });
    expect(result.current).toBe("two");
  });
});
