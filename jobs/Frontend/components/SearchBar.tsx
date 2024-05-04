"use client";

import { usePathname, useRouter, useSearchParams } from "next/navigation";
import styled from "styled-components";
import { useDebouncedCallback } from "use-debounce";

const SearchBarInput = styled.input`
  color: ${(props) => props.theme.primary.text};
  border: 1px solid ${(props) => props.theme.primary.border};
  border-radius: 5px;
  padding: 5px;
`;

const SearchBar = () => {
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();

  const handleSearch = useDebouncedCallback((term: string) => {
    const params = new URLSearchParams(searchParams);

    params.set("page", "1");

    if (term) {
      params.set("query", term);
    } else {
      params.delete("query");
    }

    replace(`${pathname}?${params.toString()}`);
  }, 300);

  return (
    <SearchBarInput
      onChange={(event) => handleSearch(event.target.value)}
      defaultValue={searchParams.get("query")?.toString()}
    />
  );
};

export default SearchBar;

/*
 * Explaining my decisions for this component
 *
 * Data Fetching:
 * A UseEffect will re-render the component and will execute on first render
 * Additionally fetching data in useEffect is not recommended - https://react.dev/reference/react/useEffect#what-are-good-alternatives-to-data-fetching-in-effects
 * Recommendation with Next is to use queryParams, and this allows us to make the request server-side
 *
 * TODO - Debouncing:
 * To debounce the request so it's not called until the user finishes typing - https://github.com/xnimorz/use-debounce
 * Why? Prevents unnecessary trips to the API while the user is typing
 * This could be written as a custom hook that is shared and to save on import size, but for the purposes of this exercise I'd prefer to save the time
 */
