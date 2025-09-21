import type { Configuration } from "@/entities/configuration/types";
import type { MovieListItem } from "@/entities/movie/types";
import type { MovieCardItem } from "@/pages/movies-list/types";
import { toLocaleDate } from "@/shared/lib/utils";

export function moviesToMovieCards({
  movies,
  configuration,
  language,
}: {
  movies: MovieListItem[];
  configuration: Configuration;
  language: string;
}): MovieCardItem[] {
  const posterBaseUrl =
    configuration.images.base_url + configuration.images.poster_sizes[1];
  return movies.map((movie) => {
    return {
      movie,
      poster_img: movie.poster_path ? posterBaseUrl + movie.poster_path : "",
      release_date_locale:
        movie.release_date !== ""
          ? toLocaleDate(movie.release_date, language)
          : "Not available",
    };
  });
}
