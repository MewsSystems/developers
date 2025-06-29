import { useInfiniteQuery } from "@tanstack/react-query";
import { getPopularMovies } from "../api/requests";
import type { PopularMoviesResponse } from "../api/types";

export const usePopularMoviesQuery = () => {
  return useInfiniteQuery<PopularMoviesResponse, Error>({
    queryKey: ["popularMovies"],
    queryFn: ({ pageParam }) => getPopularMovies(pageParam as number),
    initialPageParam: 1,
    getNextPageParam: (lastPage) => lastPage.page + 1,
  });
};
