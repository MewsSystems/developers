import {useEffect, useRef} from 'react';
import {useSearchInput} from '../../../../hooks/useSearchInput';
import {ClearButton, SearchContainer, SearchInput, SearchWarning} from './SearchInput.styled';

/*
  "MAX_USER_INPUT_SEARCH_LENGTH" search query has been limited to 100 chars since most movie titles
  are well under this length. This helps to prevent abusing the API.
*/
export const MAX_USER_INPUT_SEARCH_LENGTH = 100;

export default function SearchBar() {
  const {searchUrlParam, onSearchInputChange, clearSearch} = useSearchInput();
  const searchInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    searchInputRef.current?.focus();
  }, []);

  const onClearClick = () => {
    clearSearch();
    searchInputRef.current?.focus();
  };

  const SEARCH_INPUT_WARNING_THRESHOLD = 90;
  const charactersLeft = MAX_USER_INPUT_SEARCH_LENGTH - searchUrlParam.length;

  return (
    <SearchContainer>
      <SearchInput
        ref={searchInputRef}
        type="text"
        value={searchUrlParam}
        onChange={onSearchInputChange}
        placeholder="Start typing..."
        maxLength={MAX_USER_INPUT_SEARCH_LENGTH}
      />

      {searchUrlParam && (
        <ClearButton onClick={onClearClick} type="button" aria-label="Clear search">
          Clear
          <svg fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              strokeWidth={2}
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
        </ClearButton>
      )}

      {searchUrlParam.length >= SEARCH_INPUT_WARNING_THRESHOLD && (
        <SearchWarning isError={charactersLeft === 0}>
          {charactersLeft === 0
            ? "You've reached the limit of 100 characters."
            : `${charactersLeft} characters remaining`}
        </SearchWarning>
      )}
    </SearchContainer>
  );
}

SearchBar.displayName = 'SearchBar';
