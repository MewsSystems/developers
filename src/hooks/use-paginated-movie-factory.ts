import { useState, useEffect, useCallback } from "react";
import type { MovieApiResponse, Movie } from "~/types/movie";

/*
  Defines the shape that our query hook should have.
  It takes in variables + options (enabled), and returns data + isLoading.
*/
type UseQueryHook<TInput> = (
  input: TInput,
  options?: { enabled: boolean },
) => { data?: MovieApiResponse; isLoading: boolean };

export function createUsePaginatedMoviesHook<TInput extends { page?: number }>(
  useQueryHook: UseQueryHook<TInput>,
) {
  return function usePaginatedMovies(
    parentInput: Omit<TInput, "page">,
    isEnabled: boolean,
  ) {
    const [movies, setMovies] = useState<Movie[]>([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState(1);
    const [isLoading, setIsLoading] = useState(false);

    // Combine parent input + page number to make our final input object
    const mergedInput = {
      ...parentInput,
      page: currentPage,
    } as TInput;

    // input for prefetching the next page
    const nextPageInput = {
      ...parentInput,
      page: currentPage + 1,
    } as TInput;

    // API call for the current page
    const { data, isLoading: queryLoading } = useQueryHook(mergedInput, {
      enabled: isEnabled,
    });

    /*
     * Prefetch next page while user is still on the current page
     * (improves user experience)
     */
    useQueryHook(nextPageInput, {
      enabled: isEnabled && currentPage < totalPages,
    });

    // Sync local isLoading state with query hook's loading state
    useEffect(() => {
      setIsLoading(queryLoading);
    }, [queryLoading]);

    useEffect(() => {
      if (!data || !isEnabled) return;
      const { results, total_pages } = data;
      setMovies(results);
      setTotalPages(total_pages);
    }, [data, isEnabled]);

    const resetPage = useCallback(() => {
      setCurrentPage(1);
    }, []);

    return {
      movies,
      page: currentPage,
      totalPages,
      isLoading,
      setPage: setCurrentPage,
      resetPage,
      setMovies,
    };
  };
}
