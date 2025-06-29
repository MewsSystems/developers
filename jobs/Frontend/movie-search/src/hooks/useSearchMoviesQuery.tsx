import { keepPreviousData, useInfiniteQuery } from "@tanstack/react-query";
import type { PopularMoviesResponse } from "../api/types";
import { getSearchMovies } from "../api/requests";

export const useSearchMoviesQuery = (searchTerm: string) => {
  return useInfiniteQuery<PopularMoviesResponse, Error>({
    queryKey: ["searchMovies", searchTerm],
    queryFn: ({ pageParam = 1 }) =>
      getSearchMovies(searchTerm, pageParam as number),
    enabled: Boolean(searchTerm),
    placeholderData: keepPreviousData,
    initialPageParam: 1,
    getNextPageParam: (lastPage) =>
      lastPage.page < lastPage.total_pages ? lastPage.page + 1 : undefined,
  });
};
