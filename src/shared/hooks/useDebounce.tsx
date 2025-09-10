import { useCallback, useRef } from "react";

/**
 * Custom hook for debounced search functionality
 * @param callback - Function to call after debounce delay
 * @param delay - Debounce delay in milliseconds
 * @returns Debounced function
 */
export const useDebounce = (
  callback: (query: string, abortController?: AbortController) => void,
  delay: number
) => {
  const timeoutRef = useRef<number | null>(null);
  const abortControllerRef = useRef<AbortController | null>(null);

  const debouncedCallback = useCallback(
    (query: string) => {
      // Cancel previous request if exists
      if (abortControllerRef.current) {
        abortControllerRef.current.abort();
      }

      // Clear previous timeout
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }

      // Set new timeout
      timeoutRef.current = window.setTimeout(() => {
        // Create new AbortController for this request
        abortControllerRef.current = new AbortController();
        callback(query, abortControllerRef.current);
      }, delay);
    },
    [callback, delay]
  );

  // Cleanup function
  const cleanup = useCallback(() => {
    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }
    if (abortControllerRef.current) {
      abortControllerRef.current.abort();
    }
  }, []);

  return { debouncedCallback, cleanup };
};