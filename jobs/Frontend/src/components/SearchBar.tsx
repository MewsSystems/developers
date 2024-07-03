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
  console.log("load the search bar");

  const doSearch = useCallback((value: string) => {
    console.log("Changed value:", value);
    onSearch(value);
  }, []);

  // memoize the debounce call with useMemo
  const debouncedSearch = useMemo(() => {
    return debounce(doSearch, 1000);
  }, [doSearch]);

  return (
    <Input
      placeholder="Search for a movie..."
      onChange={(e) => {
        console.log("typing:", e.target.value);
        debouncedSearch(e.target.value);
      }}
    />
  );
});

export default SearchBar;
