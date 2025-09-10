import { LoaderPinwheel, SearchIcon } from 'lucide-react';
import { useSearchbar } from '../hooks/useSearchbar';

/**
 * Props interface for SearchBar component
 */
interface SearchBarProps {
  /** Callback function triggered when search is performed */
  onSearch: (query: string, abortController?: AbortController) => void;
  /** Initial search query value */
  initialQuery?: string;
  /** Loading state indicator */
  isLoading?: boolean;
  /** Error message to display */
  error?: string | null;
  /** Debounce delay in milliseconds */
  debounceDelay?: number;
  /** Placeholder text for the input field */
  placeholder?: string;
  /** Disabled state */
  disabled?: boolean;
}



/**
 * Enhanced search bar component with debounce functionality and accessibility features
 */
export const SearchBar = ({
  onSearch,
  initialQuery = '',
  isLoading = false,
  error = null,
  debounceDelay = 400,
  placeholder = 'Search movies...',
  disabled = false,
}: SearchBarProps) => {
  // Use custom hook for all search logic
  const {
    query,
    isFocused,
    inputRef,
    handleInputChange,
    handleFocus,
    handleBlur,
    handleKeyDown,
    clearSearch,
  } = useSearchbar({
    onSearch,
    initialQuery,
    debounceDelay,
  });

  // Dynamic classes for responsive design and states
  const containerClasses = `
    relative w-full max-w-2xl mx-auto mb-8
    ${disabled ? 'opacity-50 cursor-not-allowed' : ''}
  `.trim();

  const inputContainerClasses = `
    relative flex items-center
    border-2 rounded-lg overflow-hidden transition-all duration-200
    ${error 
      ? 'border-red-500 focus-within:border-red-600 focus-within:ring-2 focus-within:ring-red-200'
      : isFocused 
        ? 'border-blue-500 focus-within:border-blue-600 focus-within:ring-2 focus-within:ring-blue-200'
        : 'border-gray-300 hover:border-gray-400'
    }
    ${disabled ? 'bg-gray-100' : 'bg-white'}
  `.trim();

  const inputClasses = `
    flex-grow px-4 py-3 text-base
    focus:outline-none transition-colors duration-200
    ${disabled ? 'bg-gray-100 cursor-not-allowed' : 'bg-transparent'}
    placeholder:text-gray-500
    sm:text-sm md:text-base
  `.trim();

  return (
    <div className={containerClasses}>
      <div className={inputContainerClasses}>
        {/* Search Icon */}
        <div className="pl-4 pr-2 flex items-center">
          <svg
            className={`w-5 h-5 transition-colors duration-200 ${
              isLoading ? 'animate-spin text-blue-500' : 'text-gray-400'
            }`}
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
            xmlns="http://www.w3.org/2000/svg"
            aria-hidden="true"
          >
            {isLoading ? (
              <LoaderPinwheel />
            ) : (
              <SearchIcon />
            )}
          </svg>
        </div>

        {/* Input Field */}
        <input
          ref={inputRef}
          type="text"
          value={query}
          onChange={handleInputChange}
          onFocus={handleFocus}
          onBlur={handleBlur}
          onKeyDown={handleKeyDown}
          placeholder={placeholder}
          disabled={disabled}
          className={inputClasses}
          // Accessibility attributes
          role="searchbox"
          aria-label="Search movies"
          aria-describedby={error ? 'search-error' : undefined}
          aria-invalid={error ? 'true' : 'false'}
          autoComplete="off"
          autoCorrect="off"
          autoCapitalize="off"
          spellCheck="false"
        />

        {/* Clear Button */}
        {query && !disabled && (
          <button
            type="button"
            onClick={clearSearch}
            className="p-2 mr-2 text-gray-400 hover:text-gray-600 transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-blue-200 rounded"
            aria-label="Clear search"
          >
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
              xmlns="http://www.w3.org/2000/svg"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M6 18L18 6M6 6l12 12"
              />
            </svg>
          </button>
        )}
      </div>

      {/* Error Message */}
      {error && (
        <div
          id="search-error"
          className="mt-2 text-sm text-red-600 px-4"
          role="alert"
          aria-live="polite"
        >
          {error}
        </div>
      )}
    </div>
  );
};