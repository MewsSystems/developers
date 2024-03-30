import Grid from "@mui/material/Grid";
import { useAppSelector } from "../../../redux/hooks";
import { moviesSelectors } from "../../../redux/movies/movies.slice.selectors";
import { MovieListItem } from "./MovieListItem";

export const MovieList = () => {
  const movies = useAppSelector(moviesSelectors.getMovies);

  return (
    <Grid container spacing={2}>
      {movies.map((movie) => (
        <Grid key={movie.id} item xs={6} sm={4}>
          <MovieListItem movie={movie} />
        </Grid>
      ))}
    </Grid>
  );
};
