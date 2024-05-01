import { useInfiniteQuery } from "@tanstack/react-query";
import { HttpError, findMovies } from "src/api/fmdb";

function useMoviesQuery(query: string) {
  const queryResult = useInfiniteQuery({
    queryKey: ["movies", query],
    queryFn: async ({ pageParam, queryKey: [_, query], signal }) =>
      findMovies(query, pageParam, { signal }),
    initialPageParam: 1,
    getNextPageParam: (lastPage, _) =>
      lastPage.page < lastPage.total_pages ? lastPage.page + 1 : undefined,
    // only server errors will go to the Error Boundary
    throwOnError: (error) => error instanceof HttpError && error.status >= 500,
  });

  const movies = queryResult.data?.pages.flatMap((page) => page.results) || [];
  return { ...queryResult, movies };
}

export default useMoviesQuery;
