import type { MovieDetailsAppended } from "@/entities/movie/types";
import {
  toDurationFormat,
  toLocaleDate,
  toLocaleYear,
} from "@/shared/lib/utils";
import type { MovieTitle } from "@/features/title/types";

export function parseTitle({
  movie,
  language,
}: {
  movie: MovieDetailsAppended;
  language: string;
}): MovieTitle {
  return {
    duration: toDurationFormat(movie.runtime),
    countriesOrigin: movie.origin_country.map((c) => c).join(", "),
    genres: movie.genres.map((g) => g.name).join(", "),
    releaseDateLocale: toLocaleDate(movie.release_date, language),
    releaseDateYearLocale: toLocaleYear(movie.release_date, language),
    title: movie.title,
  };
}
