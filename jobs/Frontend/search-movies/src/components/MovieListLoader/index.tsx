import styled from "styled-components";
import { useAppSelector } from "../../store/hooks";
import { colors } from "../../utils/theme";

const LoadingContainer = styled.div`
  color: ${colors.primaryText};
  display: flex;
  justify-content: center;
  height: 78vh;
  margin: auto;
`;

interface MovieListLoaderProps {
  page: string;
}

/**
 * Loader to render while the movie list is loaded from the redux state
 * @param props {page} page number to access the list of movies on a page
 * @returns renders loader if the movie list is empty for a page
 */
const MovieListLoader = (props: MovieListLoaderProps) => {
  const { page } = props;
  const browseMovies = useAppSelector((state) => state.browseMovies);

  return (
    <>
      {!browseMovies.pages[page] && (
        <LoadingContainer>Loading...</LoadingContainer>
      )}
    </>
  );
};

export default MovieListLoader;
