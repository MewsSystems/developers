import { CircularProgress } from "@mui/material";
import { MovieDetails } from "../components/MovieDetails";
import { useMovieDetails } from "../hooks/useMovieDetails";
import { useAppSelector } from "../redux/hooks";
import { moviesSelectors } from "../redux/movies/movies.slice.selectors";

export const MovieDetailsView = () => {
  useMovieDetails();
  const selectedMovie = useAppSelector(moviesSelectors.getSelectedMovie);

  if (!selectedMovie) {
    return <CircularProgress />;
  }

  return <MovieDetails />;
};
