import { useCallback, useRef } from "react";

// Define the hook with generic type parameters
function useDebounce<T extends unknown[]>(
  callback: (...args: T) => void,
  delay: number
): [(...args: T) => void, () => void] {
  const timerRef = useRef<number | null>(null);

  // This function clears the current timer, preventing the callback from executing if it was set.
  const cancel = useCallback(() => {
    if (timerRef.current) {
      window.clearTimeout(timerRef.current);
      timerRef.current = null;
    }
  }, []);

  // The debounced function that applies the debounce effect to the callback
  const debouncedCallback = useCallback(
    (...args: T) => {
      // Cancel any existing timers before setting a new one
      cancel();
      // Set a new timer
      timerRef.current = window.setTimeout(() => {
        callback(...args);
      }, delay);
    },
    [callback, delay, cancel]
  );

  // Return both the debounced callback and the cancel function so it can be used to cancel the timer if needed
  return [debouncedCallback, cancel];
}

export default useDebounce;
