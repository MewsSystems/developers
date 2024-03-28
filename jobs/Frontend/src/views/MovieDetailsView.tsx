import { MovieDetails } from "../components/MovieDetails";
import { useMovieDetails } from "../hooks/useMovieDetails";

export const MovieDetailsView = () => {
  useMovieDetails();

  return <MovieDetails />;
};
