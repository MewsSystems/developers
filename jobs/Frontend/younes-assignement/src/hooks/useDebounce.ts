import { useState, useEffect } from "react";

/**
 * Returns a debounced version of a value.
 *
 * @param value - The value to debounce
 * @param delay - Debounce delay in milliseconds (default 500ms)
 */
const useDebounce = <T>(value: T, delay = 500): T => {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    return () => clearTimeout(timer);
  }, [value, delay]);

  return debouncedValue;
};

export default useDebounce;
