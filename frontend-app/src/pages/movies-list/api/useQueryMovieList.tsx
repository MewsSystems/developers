import useQueryConfiguration from "@/entities/configuration/hooks/useQueryConfiguration";
import useQueryMovies from "@/entities/movie/hooks/useQueryMovies";
import { usePreferredLanguage } from "@uidotdev/usehooks";
import { moviesToMovieCards } from "@/pages/movies-list/lib/parseMoviesToCardMovies";

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
