import { useCallback, useEffect, useRef, useState } from 'react';
import type { ChangeEvent } from 'react';
import { useDebounce } from '@/shared/hooks/useDebounce';

/**
 * Props interface for useSearchbar hook
 */
interface UseSearchbarProps {
  /** Callback function triggered when search is performed */
  onSearch: (query: string, abortController?: AbortController) => void;
  /** Initial search query value */
  initialQuery?: string;
  /** Debounce delay in milliseconds */
  debounceDelay?: number;
}

/**
 * Return type for useSearchbar hook
 */
interface UseSearchbarReturn {
  /** Current search query */
  query: string;
  /** Input focus state */
  isFocused: boolean;
  /** Input ref for direct manipulation */
  inputRef: React.RefObject<HTMLInputElement | null>;
  /** Handle input change */
  handleInputChange: (e: ChangeEvent<HTMLInputElement>) => void;
  /** Handle input focus */
  handleFocus: () => void;
  /** Handle input blur */
  handleBlur: () => void;
  /** Handle keyboard events */
  handleKeyDown: (e: React.KeyboardEvent<HTMLInputElement>) => void;
  /** Clear search query */
  clearSearch: () => void;
}

/**
 * Custom hook for managing search bar state and behavior
 * Encapsulates all search-related logic including debouncing, keyboard navigation, and state management
 */
export const useSearchbar = ({
  onSearch,
  initialQuery = '',
  debounceDelay = 400,
}: UseSearchbarProps): UseSearchbarReturn => {
  // State management
  const [query, setQuery] = useState<string>(initialQuery);
  const [isFocused, setIsFocused] = useState<boolean>(false);
  const inputRef = useRef<HTMLInputElement>(null);

  /**
   * Handle search execution with debounce
   */
  const handleSearch = useCallback(
    (searchQuery: string, abortController?: AbortController) => {
      const trimmedQuery = searchQuery.trim();
      if (trimmedQuery.length > 0) {
        onSearch(trimmedQuery, abortController);
      }
    },
    [onSearch]
  );

  // Setup debounced search
  const { debouncedCallback: debouncedSearch, cleanup } = useDebounce(
    handleSearch,
    debounceDelay
  );

  /**
   * Handle input change with immediate UI update and debounced search
   */
  const handleInputChange = useCallback(
    (e: ChangeEvent<HTMLInputElement>) => {
      const newValue = e.target.value;
      setQuery(newValue);
      
      // Trigger debounced search or clear search
      if (newValue.trim().length > 0) {
        debouncedSearch(newValue);
      } else {
        // When input is cleared, notify parent immediately
        cleanup();
        onSearch('');
      }
    },
    [debouncedSearch, cleanup, onSearch]
  );

  /**
   * Handle input focus
   */
  const handleFocus = useCallback(() => {
    setIsFocused(true);
  }, []);

  /**
   * Handle input blur
   */
  const handleBlur = useCallback(() => {
    setIsFocused(false);
  }, []);

  /**
   * Clear search query and notify parent
   */
  const clearSearch = useCallback(() => {
    setQuery('');
    cleanup();
    onSearch('');
    inputRef.current?.focus();
  }, [cleanup, onSearch]);

  /**
   * Handle keyboard navigation
   */
  const handleKeyDown = useCallback(
    (e: React.KeyboardEvent<HTMLInputElement>) => {
      if (e.key === 'Escape') {
        // Clear search and blur input
        setQuery('');
        inputRef.current?.blur();
        cleanup();
        onSearch('');
      } else if (e.key === 'Enter') {
        // Immediate search on Enter
        e.preventDefault();
        cleanup();
        handleSearch(query);
      }
    },
    [query, handleSearch, cleanup, onSearch]
  );

  // Cleanup on unmount
  useEffect(() => {
    return cleanup;
  }, [cleanup]);

  // Update query when initialQuery changes
  useEffect(() => {
    setQuery(initialQuery);
  }, [initialQuery]);

  return {
    query,
    isFocused,
    inputRef,
    handleInputChange,
    handleFocus,
    handleBlur,
    handleKeyDown,
    clearSearch,
  };
};