import { useState, useEffect } from 'react';

export function useSearchDebounce<T>(value: T, delay: number = 500): T {
  const [debouncedValue, setDebouncedValue] = useState<T>(value);

  useEffect(() => {
    // Set a timer to update the debounced value after the specified delay
    const timer = setTimeout(() => {
      setDebouncedValue(value);
    }, delay);

    // Cleanup function to clear the timer if the value or delay changes
    return () => {
      clearTimeout(timer);
    };
    // This effect depends on value and delay
  }, [value, delay]);

  return debouncedValue;
}
