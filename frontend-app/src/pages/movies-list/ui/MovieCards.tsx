import { MovieCard } from "./MovieCard";
import type { MovieCardItem } from "../types";

export function MoviesCards({
  movieCardItems,
}: {
  movieCardItems: MovieCardItem[];
}) {
  return movieCardItems.map((movieCardItem) => (
    <MovieCard key={movieCardItem.movie.id} movieCardItem={movieCardItem} />
  ));
}
