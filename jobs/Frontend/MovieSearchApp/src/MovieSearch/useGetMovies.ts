import { useQuery } from "@tanstack/react-query";
import { QueryOptionsWithoutKeyAndFn } from "../../types";
import { moviesUrlEndpoint, apiKey } from "../../config";

export type Movies = {
  page: number;
  total_pages: number;
  total_results: number;
  results: Array<
    Partial<{
      adult: boolean;
      backdrop_path: string;
      genre_ids: Array<number>;
      original_language: string;
      original_title: string;
      overview: string;
      popularity: number;
      poster_path: string;
      release_date: string;
      title: string;
      video: boolean;
      vote_average: number;
      vote_count: number;
    }> & { id: number }
  >;
};

export type FetchMoviesQueryParams = {
  title: string;
  page?: number;
};

/**
 *
 * @param queryParams
 * @returns
 */
const fetchMovies = async (queryParams: FetchMoviesQueryParams): Promise<Movies> => {
  const response = await fetch(
    `${moviesUrlEndpoint}?api_key=${apiKey}&query=${encodeURI(queryParams.title)}&page=${queryParams.page}`
  );

  if (!response.ok) {
    throw new Error("Failed to fetch movies!");
  }

  return response.json();
};

type UseGetMoviesProps = QueryOptionsWithoutKeyAndFn<Movies> & {
  movieTitle: string;
  page: number;
};

export const useGetMovies = ({ movieTitle, page, ...queryOptions }: UseGetMoviesProps) =>
  useQuery<Movies>({
    queryKey: ["movies", movieTitle, page],
    queryFn: () => fetchMovies({ title: movieTitle, page }),
    ...queryOptions,
  });
