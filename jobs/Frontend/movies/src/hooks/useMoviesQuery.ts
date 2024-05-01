import { useInfiniteQuery } from "@tanstack/react-query";
import { findMovies } from "src/api/fmdb";

function useMoviesQuery(query: string) {
  const queryResult = useInfiniteQuery({
    queryKey: ["movies", query],
    queryFn: async ({ pageParam, queryKey: [_, query], signal }) =>
      findMovies(query, pageParam, { signal }),
    initialPageParam: 1,
    getNextPageParam: (lastPage, _) =>
      lastPage.page < lastPage.total_pages ? lastPage.page + 1 : undefined,
  });

  const movies = queryResult.data?.pages.flatMap((page) => page.results) || [];
  return { ...queryResult, movies };
}

export default useMoviesQuery;
