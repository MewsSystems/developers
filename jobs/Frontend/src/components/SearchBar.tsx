import React, { useCallback, useMemo, memo } from "react";
import debounce from "lodash/debounce";
import styled from "styled-components";

const Input = styled.input`
  width: 100%;
  padding: 8px;
  margin: 16px 0;
  font-size: 16px;
`;

interface SearchBarProps {
  onSearch: (query: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = memo(({ onSearch }) => {
  const doSearch = useCallback((value: string) => {
    onSearch(value);
  }, []);

  // memoize the debounce call with useMemo
  // so it doesn't get recreated on every render
  const debouncedSearch = useMemo(() => {
    return debounce(doSearch, 500);
  }, [doSearch]);

  return (
    <Input
      placeholder="Search for a movie title..."
      onChange={(e) => {
        debouncedSearch(e.target.value);
      }}
    />
  );
});

export default SearchBar;
