import { useEffect, useState } from "react";

/**
 * Debounce a state value. Only after the specified delay time has passed without the state value
 * changing, will be the debounced value updated. Default delay is 300ms.
 * @param value initial state value.
 * @param delay debounce delay in milliseconds
 * @returns current state value, debounced value and state update function.
 */
export const useDebounceState = <T>(value: T, delay: number = 300): [T, T, (value: T) => void] => {
  const [currentValue, setCurrentValue] = useState<T>(value);
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const timeoutId = window.setTimeout(() => {
      setDebouncedValue(currentValue);
    }, delay);

    return () => {
      clearTimeout(timeoutId);
    };
  }, [delay, currentValue]);

  return [currentValue, debouncedValue, setCurrentValue];
};
