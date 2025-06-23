import { useMemo } from "react";
import { keepPreviousData, useInfiniteQuery } from "@tanstack/react-query";
import { useSearchParams } from "react-router-dom";
import { getPopularMovies, getSearchedMovies } from "../api";

import { useDebounce } from "./useDebounce";

export function useMoviesQuery() {
  const [searchParams] = useSearchParams();
  const query = searchParams.get("query");

  const debouncedQuery = useDebounce(query, 500);

  const {
    data,
    error,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isLoading,
    isError,
  } = useInfiniteQuery({
    queryKey: ["movies", debouncedQuery],
    queryFn: ({ pageParam }) =>
      debouncedQuery
        ? getSearchedMovies(debouncedQuery, pageParam)
        : getPopularMovies(pageParam),
    initialPageParam: 1,
    getNextPageParam: (lastPage, _) =>
      lastPage.page < lastPage.total_pages ? lastPage.page + 1 : undefined,
    placeholderData: keepPreviousData,
  });

  const movies = useMemo(
    () => data?.pages.flatMap((page) => page.results),
    [data]
  );

  return {
    movies,
    error,
    fetchNextPage,
    hasNextPage,
    isFetching,
    isLoading,
    isError,
  };
}
