import { renderHook, act } from "@testing-library/react";
import { useDebounce } from "./useDebounce";

jest.useFakeTimers();

describe("useDebounce", () => {
	it("should return the initial value immediately", () => {
		const { result } = renderHook(() => useDebounce("initial", 500));
		expect(result.current).toBe("initial");
	});

	it("should update the debounced value after the delay", () => {
		const { result, rerender } = renderHook(
			({ value }) => useDebounce(value, 500),
			{
				initialProps: { value: "initial" },
			}
		);

		rerender({ value: "updated" });

		// Initially, it should return the previous value
		expect(result.current).toBe("initial");

		// Fast-forward the timers
		act(() => {
			jest.advanceTimersByTime(500);
		});

		// The debounced value should update
		expect(result.current).toBe("updated");
	});

	it("should reset the timeout when value changes before delay ends", () => {
		const { result, rerender } = renderHook(
			({ value }) => useDebounce(value, 500),
			{
				initialProps: { value: "initial" },
			}
		);

		rerender({ value: "updated1" });

		// Advance time but not enough to trigger the debounce
		act(() => {
			jest.advanceTimersByTime(300);
		});

		rerender({ value: "updated2" });

		// Still returns the initial value because the timer resets
		expect(result.current).toBe("initial");

		// Advance time to trigger the debounce
		act(() => {
			jest.advanceTimersByTime(500);
		});

		expect(result.current).toBe("updated2");
	});
});
