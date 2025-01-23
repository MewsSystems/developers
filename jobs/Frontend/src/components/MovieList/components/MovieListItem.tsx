import { FC } from "react";
import { PartialMovie } from "../../../redux/movies/movies.slice.types";
import { MovieCard } from "../../MovieCard";

export const MovieListItem: FC<{ movie: PartialMovie }> = ({ movie }) => {
  return <MovieCard movie={movie} />;
};
