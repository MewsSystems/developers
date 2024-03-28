import Box from "@mui/material/Box";
import { useAppSelector } from "../../../redux/hooks";
import { moviesSelectors } from "../../../redux/movies/movies.slice.selectors";
import { CircularProgress, Typography } from "@mui/material";
import { MovieDetailsCard } from "./MovieDetailsCard";

export const MovieDetails = () => {
  const selectedMovie = useAppSelector(moviesSelectors.getSelectedMovie);

  if (!selectedMovie) {
    return <CircularProgress />;
  }

  return (
    <Box
      display="flex"
      alignItems="center"
      flexDirection="column"
      px={4}
      py={2}
    >
      <Box overflow="auto">
        <Box
          position="sticky"
          top={0}
          sx={{ backgroundColor: "white" }}
          textAlign="center"
        >
          <Typography variant="h4">{selectedMovie.title}</Typography>
          {selectedMovie.tagline && (
            <Typography variant="subtitle1" gutterBottom>
              {selectedMovie.tagline}
            </Typography>
          )}
        </Box>
        <MovieDetailsCard />
      </Box>
    </Box>
  );
};
