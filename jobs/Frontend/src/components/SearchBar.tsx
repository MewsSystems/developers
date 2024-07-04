import React, { useCallback, useMemo, memo, useState } from "react";
import debounce from "lodash/debounce";
import styled from "styled-components";

const Input = styled.input`
  width: calc(100% - 32px);
  padding: 8px;
  margin: 16px 0;
  font-size: 16px;
`;

interface SearchBarProps {
  initialValue: string;
  onSearch: (query: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = memo(
  ({ initialValue, onSearch }) => {
    const [searchTerms, setSearchTerms] = useState(initialValue);

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
        value={searchTerms}
        placeholder="Search for a movie title..."
        onChange={(e) => {
          setSearchTerms(e.target.value);
          debouncedSearch(e.target.value);
        }}
      />
    );
  },
);

export default SearchBar;
