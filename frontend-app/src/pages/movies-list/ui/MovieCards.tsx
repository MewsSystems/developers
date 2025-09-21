import { MovieCard } from "./MovieCard";
import type { MovieCardItem } from "../types";
import { Grid } from "@chakra-ui/react";

export function MoviesCards({
  movieCardItems,
  favoritesMap,
}: {
  movieCardItems: MovieCardItem[];
  favoritesMap: Map<number, boolean>;
}) {
  return (
    <Grid templateColumns="1fr" gap="4" >
      {" "}
      {movieCardItems.map((movieCardItem) => (
        <MovieCard
          key={movieCardItem.movie.id}
          movieCardItem={movieCardItem}
          favoritesMap={favoritesMap}
        />
      ))}{" "}
    </Grid>
  );
}
