import { useInfiniteQuery } from "@tanstack/react-query";
import { getPopularMovies } from "../api/requests";
import type { PopularMoviesResponse } from "../api/types";

export const usePopularMoviesQuery = () =>
  useInfiniteQuery<PopularMoviesResponse, Error>({
    queryKey: ["popularMovies"],
    queryFn: ({ pageParam = 1 }) => getPopularMovies(pageParam as number),
    initialPageParam: 1,
    getNextPageParam: (lastPage, _, lastPageParam) => {
      if (lastPage.total_pages === 0) {
        return undefined;
      }
      return (lastPageParam as number) + 1;
    },
  });
