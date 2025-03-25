import { useRef, useState, useEffect, useCallback } from "react";
import { usePopularMovies } from "~/hooks/use-popular-movies";
import { useSearchMovies } from "~/hooks/use-search-movies";
import { type Movie } from "~/types/movie";

type Mode = "search" | "popular";

/**
 * Custom hook to handle both search movies and popular movies data.
 * This hook manages switching between "search" and "popular" modes,
 * handles loading states, and provides helpers for pagination.
 */
export function useMovieData(
  debouncedQuery: string,
  isSearching: boolean,
  isSearchInitiated: boolean,
) {
  const currentMode = isSearching ? "search" : "popular";
  const previousModeRef = useRef<Mode>(currentMode);

  const {
    movies: searchMovies,
    page: searchPage,
    totalPages: searchTotalPages,
    isLoading: searchLoading,
    setPage: setSearchPage,
    resetPage: resetSearchPage,
    setMovies: setSearchMovies,
  } = useSearchMovies({ query: debouncedQuery }, isSearching);

  const {
    movies: popularMovies,
    page: popularPage,
    totalPages: popularTotalPages,
    isLoading: popularLoading,
    setPage: setPopularPage,
    resetPage: resetPopularPage,
  } = usePopularMovies({}, !isSearchInitiated);

  // Restore page from sessionStorage on mount
  useEffect(() => {
    const savedPage = sessionStorage.getItem("movieListPage");
    if (savedPage) {
      const pageNum = parseInt(savedPage, 10);
      if (!isNaN(pageNum) && pageNum > 0) {
        if (currentMode === "search") {
          setSearchPage(pageNum);
        } else {
          setPopularPage(pageNum);
        }
      }
      sessionStorage.removeItem("movieListPage");
    }
  }, [currentMode, setSearchPage, setPopularPage]);

  // Handle Mode Changes
  useEffect(() => {
    if (previousModeRef.current !== currentMode) {
      // On mode switch, reset the relevant hook's page
      if (currentMode === "search") {
        resetSearchPage();
      } else {
        resetPopularPage();
        resetSearchPage();
      }
      previousModeRef.current = currentMode;
    }
  }, [currentMode, resetSearchPage, resetPopularPage]);

  useEffect(() => {
    if (debouncedQuery.trim() === "") {
      resetSearchPage();
      setSearchMovies([]);
    }
  }, [debouncedQuery, resetSearchPage, setSearchMovies]);

  const isChangingMode = previousModeRef.current !== currentMode;

  const currentPage = currentMode === "search" ? searchPage : popularPage;
  const currentLoading =
    currentMode === "search" ? searchLoading : popularLoading;

  const [isActivelyLoading, setIsActivelyLoading] = useState(false);

  useEffect(() => {
    const activeLoading = currentLoading || isChangingMode;
    if (currentPage === 1 || isChangingMode) {
      if (activeLoading) {
        setIsActivelyLoading(true);
      } else {
        const t = setTimeout(() => setIsActivelyLoading(false), 300);
        return () => clearTimeout(t);
      }
    }
  }, [currentLoading, currentPage, isChangingMode]);

  const [isLoadingMore, setIsLoadingMore] = useState(false);

  useEffect(() => {
    if (currentPage > 1) {
      setIsLoadingMore(currentLoading);
    } else {
      setIsLoadingMore(false);
    }
  }, [currentPage, currentLoading]);

  const resetPage = useCallback(() => {
    if (currentMode === "search") {
      resetSearchPage();
    } else {
      resetPopularPage();
    }
  }, [currentMode, resetSearchPage, resetPopularPage]);

  const setPage = useCallback(
    (newPage: number) => {
      if (currentMode === "search") {
        setSearchPage(newPage);
      } else {
        setPopularPage(newPage);
      }
    },
    [currentMode, setSearchPage, setPopularPage],
  );

  // Decide which movies to display
  const modeMovies = currentMode === "search" ? searchMovies : popularMovies;
  const oppositeMovies =
    currentMode === "search" ? popularMovies : searchMovies;
  const isLoading = currentMode === "search" ? searchLoading : popularLoading;

  /**
   * If changing modes and the new mode is still loading,
   * but we have data in the old mode, show that until new data arrives.
   */
  let moviesToDisplay: Movie[] = [];
  if (isChangingMode && isLoading && oppositeMovies.length > 0) {
    moviesToDisplay = oppositeMovies;
  } else {
    moviesToDisplay = modeMovies;
  }

  const page = currentMode === "search" ? searchPage : popularPage;
  const MAX_ALLOWED_PAGES = 500;
  const totalPages = Math.min(
    currentMode === "search" ? searchTotalPages : popularTotalPages,
    MAX_ALLOWED_PAGES,
  );

  const hasNextPage = page < totalPages;

  return {
    page,
    totalPages,
    moviesToDisplay,
    hasNextPage,
    isLoading,
    isActivelyLoading,
    isLoadingMore,
    resetPage,
    setPage,
  };
}
