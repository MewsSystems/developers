import { useCallback, useEffect, useState } from "react";
import { useDebounce } from "~/hooks/use-debounce";

export const useSearchState = (initialQuery: string) => {
  const [query, setQuery] = useState(initialQuery);
  const debouncedQuery = useDebounce(query, 500);
  const [isSearchInitiated, setIsSearchInitiated] = useState(
    initialQuery.length > 0,
  );
  const isSearching = debouncedQuery.length > 1;

  useEffect(() => {
    if (debouncedQuery.length === 0) {
      setIsSearchInitiated(false);
    }
  }, [debouncedQuery]);

  const handleSearch = useCallback((val: string) => {
    setQuery(val);
    if (val.length > 0) {
      setIsSearchInitiated(true);
    } else {
      setIsSearchInitiated(false);
    }
  }, []);

  return {
    query,
    debouncedQuery,
    isSearchInitiated,
    isSearching,
    handleSearch,
  };
};
