import { CircularProgress } from "@mui/material";
import Box from "@mui/material/Box";
import { useAppSelector } from "../../../redux/hooks";
import { moviesSelectors } from "../../../redux/movies/movies.slice.selectors";
import { MovieDetailsCard } from "./MovieDetailsCard";
import { MovieTitle } from "./MovieTitle";

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
        <MovieTitle />
        <MovieDetailsCard />
      </Box>
    </Box>
  );
};
