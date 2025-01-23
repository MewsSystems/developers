import { useQuery, useInfiniteQuery } from "@tanstack/react-query";
import { fetchMovies, fetchMovieDetail } from "../api";

/**
 * This hook fetches the movies list based on the query string.
 */
export const useSearchMovies = (query: string) => {
  return useInfiniteQuery({
    queryKey: ["movies", query],
    queryFn: ({ pageParam = 1 }) => fetchMovies(query, pageParam),
    initialPageParam: 1,
    enabled: !!query,
    getNextPageParam: (lastPage, allPages) => {
      // Assuming `lastPage` contains a property `nextCursor` for the next page number
      if (lastPage.length > 0) {
        return allPages.length + 1;
      }
      return undefined;
    },
    staleTime: 5 * 60 * 1000, // Cache the data for 5 minutes
  });
};

/**
 * This hook fetches the movie detail based on the movie ID.
 */
export const useGetMovieDetail = (id: string) => {
  return useQuery({
    queryKey: ["movieDetail", id],
    queryFn: () => fetchMovieDetail(id),
    enabled: !!id,
  });
};
