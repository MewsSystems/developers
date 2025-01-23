import { Box, Typography, styled } from "@mui/material";
import { useAppSelector } from "../../../redux/hooks";
import { moviesSelectors } from "../../../redux/movies/movies.slice.selectors";

const StyledMovieTitleWrapper = styled(Box)`
  position: sticky;
  top: 0;
  background-color: white;
  text-align: center;
`;

export const MovieTitle = () => {
  const selectedMovie = useAppSelector(moviesSelectors.getSelectedMovie);

  if (!selectedMovie) {
    return null;
  }

  return (
    <StyledMovieTitleWrapper>
      <Typography variant="h4">{selectedMovie.title}</Typography>
      {selectedMovie.tagline && (
        <Typography variant="subtitle1" gutterBottom>
          {selectedMovie.tagline}
        </Typography>
      )}
    </StyledMovieTitleWrapper>
  );
};
