import { formatDollar } from "@/shared/lib/utils";
import { getLangNameFromCode } from "language-name-map";
import type { MovieDetailsAppended } from "@/entities/movie/types";
import type { MovieInfo } from "../types";

export function parseInfo({
  movie,
}: {
  movie: MovieDetailsAppended;
}): MovieInfo {
  return {
    imdbLink: movie.imdb_id
      ? `https://www.imdb.com/title/${movie.imdb_id}`
      : "",
    status: movie.status,
    originalLanguage: getLangNameFromCode(movie.original_language)?.name + "",
    budget: formatDollar(movie.budget),
    revenue: formatDollar(movie.revenue),
    keywords: movie.keywords.keywords,
    homepage: movie.homepage,
  };
}
