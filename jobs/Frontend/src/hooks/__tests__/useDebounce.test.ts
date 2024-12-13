import { renderHook } from "@testing-library/react";
import useDebounce from "../useDebounce";

test("should use counter", () => {
  jest.useFakeTimers();
  const mockCallback = jest.fn();

  const {
    result: { current },
  } = renderHook(() => useDebounce(mockCallback, 1000));

  current();
  jest.advanceTimersToNextTimer();

  expect(mockCallback).toHaveBeenCalledTimes(1);
  jest.useRealTimers();
});
