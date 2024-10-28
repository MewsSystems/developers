import {
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Typography,
} from "@mui/material";
import { Movie } from "../api";
import { useState } from "react";
import MovieDetails from "./MovieDetails";

export default function MovieList({
  movies,
  shownCount: currentlyShownCount,
  totalCount,
}: {
  movies: Movie[];
  shownCount: number;
  totalCount: number;
}) {
  const [movieDetail, setMoveDetail] = useState<Movie | null>(null);

  return (
    <>
      <List sx={{ overflow: "auto", scrollbarGutter: "stable" }}>
        {movies.map((movie) => (
          <ListItem>
            <ListItemButton onClick={() => setMoveDetail(movie)}>
              <ListItemText
                primary={movie.title}
                secondary={movie.release_date}
              />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
      <Typography variant="caption" sx={{ textAlign: "center" }}>
        Showing {currentlyShownCount} of {totalCount} movies
      </Typography>
      {movieDetail && (
        <MovieDetails movie={movieDetail} onClose={() => setMoveDetail(null)} />
      )}
    </>
  );
}
