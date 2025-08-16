/* Global imports */
import * as React from "react";
import { Movies } from "../components/Movies";
import styled from "styled-components";

/* Local imports */

/* Types  */

/* Local utility functions */

/* Component definition */
export const MovieListView = () => {
  const [query, setQuery] = React.useState("");
  const [search, setSearch] = React.useState("");

  React.useEffect(() => {
    const timeout = setTimeout(() => {
      return setSearch(query);
    }, 500);

    return () => clearTimeout(timeout);
  }, [query]);
  return (
    <>
      <SearchBarContainer>
        <Input
          aria-label="Search a movie"
          type="text"
          onChange={(e) => setQuery(e.currentTarget.value)}
          placeholder="Search a movie"
        />
      </SearchBarContainer>
      <Movies searchTerm={search} />
    </>
  );
};

const SearchBarContainer = styled.div`
  background: #153448;
  grid-area: search;
  justify-content: center;
  display: flex;
`;
const Input = styled.input`
  padding: 0.5rem;
  margin: 1rem;
  border: none;
  border-radius: 5px;
  width: 200px;
  font-size: 1rem;
  outline: none;
`;
