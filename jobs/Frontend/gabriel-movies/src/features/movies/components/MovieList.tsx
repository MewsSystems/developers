import { Grid } from "@/shared/ui/atoms/Layout/Grid";
import type { Movie } from "../types";
import { MovieCard } from "./MovieCard";

type MovieListProps = {
  items: Movie[]
}

export function MovieList({ items }: MovieListProps) {
  return (
    <Grid role="list">
      {items.map((m) => (
        <MovieCard key={m.id} movie={m} />
      ))}
    </Grid>
  );
}
