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
