import { Link } from "react-router-dom";
import styled from "styled-components";
import { colors, device } from "../../utils/theme";
import { Movie } from "../../utils/types";
import { shadow } from "../General";

interface SearchResultsProps {
  searchResults: Movie[];
  setSearchResults: React.Dispatch<any>;
}

const SearchResultsContainer = styled.div`
  display: static;
  position: absolute;
  text-align: left;
  width: 18rem;
  margin-left: 2rem;
  box-shadow: ${shadow};
  margin-top: 10px;
  background-color: ${colors.white};
  padding: 10px;
  border-radius: 10px;
  overflow-y: auto;
  max-height: 30rem;

  @media ${device.tablet} {
    margin-left: 0;
  }
`;

const SearchResultsUl = styled.div`
  display: flex;
  flex-direction: column;
`;
const SearchResultsLi = styled.div`
  border-bottom: 1px solid ${colors.black};
  padding: 10px;
`;
const SearchResults = (props: SearchResultsProps) => {
  const { searchResults, setSearchResults } = props;
  console.log(searchResults);
  return (
    <>
      {searchResults.length > 0 && (
        <SearchResultsContainer>
          <SearchResultsUl onClick={() => setSearchResults([])}>
            {searchResults.map((movie) => (
              <Link
                to={`/movie/${movie.id}`}
                style={{ textDecoration: "none" }}
              >
                <SearchResultsLi>{movie.original_title}</SearchResultsLi>
              </Link>
            ))}
          </SearchResultsUl>
        </SearchResultsContainer>
      )}
    </>
  );
};
export default SearchResults;
