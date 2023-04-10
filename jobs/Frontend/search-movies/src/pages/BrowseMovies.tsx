import { useParams } from "react-router";
import styled from "styled-components";
import { colors } from "../utils/theme";

const BrowseMoviesContainer = styled.div`
  color: ${colors.primaryText};
`;
const BrowseMovies = () => {
  const { pageId } = useParams();
  return (
    <>{pageId && <BrowseMoviesContainer>{pageId}</BrowseMoviesContainer>}</>
  );
};

export default BrowseMovies;
