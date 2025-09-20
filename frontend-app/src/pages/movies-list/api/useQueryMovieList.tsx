import useQueryConfiguration from "@/entities/configuration/hooks/useQueryConfiguration";
import type { Configuration } from "@/entities/configuration/types";
import useQueryMovies from "@/entities/movie/hooks/useQueryMovies";
import type { MovieListItem } from "@/entities/movie/types";
import { usePreferredLanguage } from "@uidotdev/usehooks";
import type { MovieCardItem } from "../types";
import { toLocaleDate } from "@/shared/lib/utils";

export function useQueryMovieList({
  query,
  page,
}: {
  query: string;
  page: string;
}) {
  const language = usePreferredLanguage();
  const { data, isLoading } = useQueryMovies({
    query,
    page,
    language,
  });
  const { data: dataConfiguration, isLoading: isLoadingConfiguration } =
    useQueryConfiguration();

  const movieCardItems =
    data && dataConfiguration
      ? moviesToMovieCards({
          movies: data.results,
          language,
          configuration: dataConfiguration,
        })
      : [];

  return {
    isLoading: isLoading || isLoadingConfiguration,
    data: movieCardItems,
    total_results: data?.total_results ?? 0,
    total_pages: data?.total_pages ?? 0,
  };
}

function moviesToMovieCards({
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
      release_date_locale: toLocaleDate(movie.release_date, language),
    };
  });
}
