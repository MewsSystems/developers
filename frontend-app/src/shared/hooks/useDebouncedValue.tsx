import { useCallback, useState } from "react";
import { debounce } from "@tanstack/react-pacer/debouncer";

const DEBOUNCE_TIME_MILLISECONDS = 800;
export function useDebouncedValue(
  initialValue: string
): [string, string, (a: React.ChangeEvent<HTMLInputElement>) => void] {
  const [searchText, setSearchText] = useState(initialValue);
  const [debouncedSearchText, setDebouncedSearchText] = useState(initialValue);

  const debouncedSetSearch = useCallback(
    debounce(setDebouncedSearchText, {
      wait: DEBOUNCE_TIME_MILLISECONDS,
    }),
    []
  );

  function updateValue(e: React.ChangeEvent<HTMLInputElement>) {
    const newValue = e.target.value;
    setSearchText(newValue);
    debouncedSetSearch(newValue);
  }

  return [searchText, debouncedSearchText, updateValue];
}
