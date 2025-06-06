import {memo, useCallback, useEffect, useMemo, useRef} from 'react';
import {useSearchInput} from '../../../../hooks/useSearchInput';
import {ClearButton, SearchContainer, SearchInput, SearchWarning} from './styled';

/*
  "MAX_USER_INPUT_SEARCH_LENGTH" search query has been limited to 100 chars since most movie titles
  are well under this length. This helps to prevent abusing the API.
*/
export const MAX_USER_INPUT_SEARCH_LENGTH = 100;
const SEARCH_INPUT_WARNING_THRESHOLD = 90;

function SearchBar() {
  const {searchUrlParam, onSearchInputChange, clearSearch} = useSearchInput();
  const searchInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    searchInputRef.current?.focus();
  }, []);

  const onClearClick = useCallback(() => {
    clearSearch();
    searchInputRef.current?.focus();
  }, [clearSearch]);

  const charactersLeft = useMemo(
    () => MAX_USER_INPUT_SEARCH_LENGTH - searchUrlParam.length,
    [searchUrlParam.length],
  );

  const shouldShowWarningMessage = useMemo(
    () => searchUrlParam.length >= SEARCH_INPUT_WARNING_THRESHOLD,
    [searchUrlParam.length],
  );

  const warningMessage = useMemo(() => {
    if (charactersLeft === 0) {
      return "You've reached the limit of 100 characters.";
    }
    return `${charactersLeft} characters remaining`;
  }, [charactersLeft]);

  return (
    <SearchContainer>
      <SearchInput
        ref={searchInputRef}
        type="text"
        value={searchUrlParam}
        onChange={({target: {value}}) => onSearchInputChange(value)}
        placeholder="Start typing to discover"
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

      {shouldShowWarningMessage && (
        <SearchWarning isError={charactersLeft === 0}>{warningMessage}</SearchWarning>
      )}
    </SearchContainer>
  );
}

SearchBar.displayName = 'SearchBar';

export default memo(SearchBar);
