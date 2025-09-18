import { useInfiniteQuery } from "@tanstack/react-query";
import type { Movie, TmdbSearchResponse } from "../types";
import { tmdbHeaders } from "../api/auth";
import { mapToMovie } from "../utils/mapToMovie";
import { TMDB_BASE } from "../constants";

export type UseSearchMoviesProps = {
  data: Movie[];
  isLoading: boolean;
  isFetchingNextPage: boolean;
  hasNextPage: boolean;
  fetchNextPage: () => void;
};

export function useSearchMovies(query: string): UseSearchMoviesProps {
  const {
    data,
    isLoading,
    isFetchingNextPage,
    hasNextPage,
    fetchNextPage,
  } = useInfiniteQuery<TmdbSearchResponse, Error, Movie[]>({
    queryKey: ["movies:search", { q: query }],
    initialPageParam: 1,
    enabled: !!query,
    queryFn: async ({ pageParam, signal }) => {
      const q = encodeURIComponent(query.trim());
      const url = `${TMDB_BASE}/search/movie?include_adult=false&query=${q}&page=${pageParam}`;

      const res = await fetch(url, { headers: tmdbHeaders, signal });
      if (!res.ok) {
        throw new Error("Error fetching movies");
      }
      return (await res.json()) as TmdbSearchResponse;
    },
    getNextPageParam: (data) =>
      data.page < data.total_pages ? data.page + 1 : undefined,
    select: (data) => data.pages.flatMap((p) => p.results.map(mapToMovie)),
    throwOnError: true,
  });

  return {
    data: data ?? [],
    isLoading,
    isFetchingNextPage,
    hasNextPage,
    fetchNextPage
  };
}
