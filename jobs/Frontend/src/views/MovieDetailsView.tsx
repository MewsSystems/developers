import Box from "@mui/material/Box";
import { BackToHomeButton } from "../components/BackToHomeButton";
import { MovieDetails } from "../components/MovieDetails";
import { useMovieDetails } from "../hooks/useMovieDetails";

export const MovieDetailsView = () => {
  useMovieDetails();

  return (
    <Box display="flex" flexDirection="column" alignItems="center">
      <BackToHomeButton />
      <MovieDetails />
    </Box>
  );
};
