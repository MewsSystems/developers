import { renderHook, act } from "@testing-library/react";
import { useState } from "react";
import { useDebounce } from "../../../src/hooks/useDebounce";

beforeAll(() => vi.useFakeTimers());
afterAll(() => vi.useRealTimers());

it("debounces value changes", async () => {
    const { result } = renderHook(() => {
        const [v, setV] = useState("a");
        const d = useDebounce(v, 500); // returns { debounced, isDebouncing }
        return { v, setV, d };
    });

    // spam changes within the debounce window
    act(() => {
        result.current.setV("ab");
        result.current.setV("abc");
    });

    // Before 500ms, the debounced value should still be the initial one
    act(() => {
        vi.advanceTimersByTime(400);
    });
    expect(result.current.d.debounced).toBe("a");

    // Complete the debounce and let React apply state
    await act(async () => {
        vi.advanceTimersByTime(100);
        // give microtasks a tick so state/effects settle
        await Promise.resolve();
    });

    expect(result.current.d.debounced).toBe("abc");
});
