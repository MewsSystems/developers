/* Global imports */
import * as React from "react";
import styled from "styled-components";
import { Movies } from "./Movies";

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
      <HeaderNav>
        <Title>Mews Movie Search App</Title>
        <SearchBarContainer>
          <Input
            aria-label="Search a movie"
            type="text"
            onChange={(e) => setQuery(e.currentTarget.value)}
            placeholder="Search a movie"
          />
        </SearchBarContainer>
      </HeaderNav>
      <Movies searchTerm={search} />
    </>
  );
};
const Input = styled.input`
  padding: 0.5rem;
  margin: 1rem;
  border: none;
  border-radius: 5px;
  width: 200px;
  font-size: 1rem;
  outline: none;
`;

const Title = styled.h3`
  color: white;
  padding: 0.5rem 1rem;
`;
const HeaderNav = styled.nav`
  background: #1a1a1a;
  display: flex;
  justify-content: flex-start;
  align-items: center;
  position: sticky;
  top: 0px;
  z-index: 1;
`;
const SearchBarContainer = styled.div`
  background: white;
`;
