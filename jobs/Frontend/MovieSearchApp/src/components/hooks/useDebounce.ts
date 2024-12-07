import { useEffect, useState } from "react";

/**
 * Debounce an arbitrary value. Only after the specified delay time has passed without the value
 * changing, will be the value updated. Default delay is 300ms.
 * @param value Value that you want to debounce.
 * @param delay Delay of the value update in milliseconds
 * @returns Returns the latest value.
 */
export const useDebounce = <T>(value: T, delay: number = 300): T => {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const timeoutId = window.setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => {
      clearTimeout(timeoutId);
    };
  }, [delay, value]);

  return debouncedValue;
};
