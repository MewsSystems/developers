"use client";

import { usePathname, useRouter, useSearchParams } from "next/navigation";
import styled from "styled-components";
import { useDebouncedCallback } from "use-debounce";
import { tabletMediaQuery } from "@/breakpoints";

const SearchBarInput = styled.input`
  color: ${(props) => props.theme.primary.text};
  border: 1px solid ${(props) => props.theme.primary.border};
  border-radius: 25px;
  padding: 5px 15px;
  display: flex;
  margin: 15px auto;
  width: 250px;
  height: 45px;
  font-size: 16px;

  ${tabletMediaQuery} {
    width: 350px;
    font-size: 14px;
  }
`;

const SearchBar = () => {
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();

  const handleSearch = useDebouncedCallback((term: string) => {
    const params = new URLSearchParams(searchParams);

    if (term) {
      params.set("query", term);
      params.set("page", "1");
    } else {
      params.delete("query");
      params.delete("page");
    }

    replace(`${pathname}?${params.toString()}`);
  }, 500);

  return (
    <SearchBarInput
      onChange={(event) => handleSearch(event.target.value)}
      defaultValue={searchParams.get("query")?.toString()}
      placeholder="Search for a movie"
      type="search"
      name="search"
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
