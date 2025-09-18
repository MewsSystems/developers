import { useSuspenseQuery } from "@tanstack/react-query";
import type { Movie, TmdbMovieDetailsResponse } from "../types";
import { tmdbHeaders } from "../api/auth";
import { TMDB_BASE } from "../constants";
import { mapToMovie } from "../utils/mapToMovie";

type UseFetchMovieProps = {
  data: Movie;
};

export function useFetchMovie(id: string | undefined): UseFetchMovieProps {
  return useSuspenseQuery<TmdbMovieDetailsResponse, Error, Movie>({
    queryKey: ["movies:detail", { id }],
    queryFn: async ({ signal }) => {
      const res = await fetch(`${TMDB_BASE}/movie/${id}`, {
        headers: tmdbHeaders,
        signal,
      });
      if (!res.ok) throw new Error("Error fetching movie");
      return (await res.json()) as TmdbMovieDetailsResponse;
    },
    select: mapToMovie,
  });
}
