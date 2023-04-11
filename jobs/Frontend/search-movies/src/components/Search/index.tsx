import _ from "lodash";
import { useState } from "react";
import { FaSearch } from "react-icons/fa";
import styled from "styled-components";
import { API_KEY, endpoints } from "../../config/api";
import { axiosBrowse } from "../../config/axios";
import { colors } from "../../utils/theme";
import { shadowInner } from "../General";
import SearchResults from "./SearchResults";

const StyledInputSearchBox = styled.input`
  border-radius: 26px 26px;
  outline: none;
  border: 1px solid ${colors.primary};
  font-size: 16px;
  padding: 15px 20px 15px 45px;
  font-weight: 700;
  letter-spacing: 0.07rem;
  color: ${colors.black};
  &:focus {
    box-shadow: ${shadowInner};
    border: 1px solid transparent;
    outline: none;
  }
  &::placeholder {
    opacity: 0.7;
  }
`;

const StyledSearchBox = styled.div`
  margin: auto 10px;
`;

const StyledIconBox = styled.span`
  margin: 15px 20px;
  margin-right: -5px;
  position: absolute;
  svg {
    color: ${colors.primary};
  }
`;

/**
 * Search for a movie or genre
 * @returns renders list of resulting movies for a search
 */
const Search = () => {
  const [searchResults, setSearchResults] = useState<any>([]);

  const searchMoviesAsync = async (
    queryEvent: React.ChangeEvent<HTMLInputElement>
  ) => {
    const query = queryEvent.target.value;
    try {
      const queryResults = await axiosBrowse.get(endpoints.searchMovie, {
        params: {
          api_key: API_KEY,
          query: query,
        },
      });
      setSearchResults(queryResults.data.results);
    } catch (error) {
      console.error(error);
    }
  };
  const searchMovies = _.debounce(
    (queryEvent) => searchMoviesAsync(queryEvent),
    250
  );
  return (
    <>
      <StyledSearchBox onChange={searchMovies}>
        <StyledIconBox>
          <FaSearch />
        </StyledIconBox>
        <StyledInputSearchBox placeholder="Search for movies"></StyledInputSearchBox>
        <SearchResults
          searchResults={searchResults}
          setSearchResults={setSearchResults}
        ></SearchResults>
      </StyledSearchBox>
    </>
  );
};

export default Search;
