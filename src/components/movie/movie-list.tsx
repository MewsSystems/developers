"use client";

import { useCallback, useEffect } from "react";
import { SearchBar } from "~/components/search-bar";
import MovieCard from "~/components/movie/movie-card";
import { useRouter } from "next/navigation";
import { Loader } from "~/components/ui/loader";
import { useUrlParams } from "~/hooks/use-url-params";
import { useSearchState } from "~/hooks/use-search-state";
import { useMovieData } from "~/hooks/use-movie-data";
import { useMovieNavigation } from "~/hooks/use-movie-navigation";
import { EmptyState } from "~/components/movie/empty-state";
import { PaginationWrapper } from "~/components/pagination-wrapper";

export default function MovieList() {
  const router = useRouter();
  const { queryParam, updateUrl } = useUrlParams();
  const {
    query,
    debouncedQuery,
    isSearchInitiated,
    isSearching,
    handleSearch,
  } = useSearchState(queryParam);

  const {
    page,
    resetPage,
    isLoading,
    isActivelyLoading,
    totalPages,
    moviesToDisplay,
    setPage,
  } = useMovieData(debouncedQuery, isSearching, isSearchInitiated);

  const { isNavigating, handleMovieClick } = useMovieNavigation(router, page);

  const handleSearchWithPageReset = useCallback(
    (val: string) => {
      handleSearch(val);
      resetPage();
    },
    [handleSearch, resetPage],
  );

  const handlePageChange = (newPage: number) => {
    if (newPage === page) return;
    setPage(newPage);
  };

  useEffect(() => {
    updateUrl(debouncedQuery, page);
  }, [debouncedQuery, page, updateUrl]);

  useEffect(() => {
    if (!isLoading && moviesToDisplay.length > 0) {
      window.scrollTo({ top: 0, behavior: "smooth" });
    }
  }, [isLoading, moviesToDisplay]);

  return (
    <div className="p-4">
      <SearchBar value={query} onChange={handleSearchWithPageReset} />

      <div className="relative">
        {!moviesToDisplay.length && !isLoading ? (
          <EmptyState />
        ) : (
          <div className="grid grid-cols-2 gap-4 md:grid-cols-4">
            {moviesToDisplay.map((movie) => (
              <MovieCard
                key={movie.id}
                movie={movie}
                onClick={() => handleMovieClick(movie.id)}
              />
            ))}
          </div>
        )}

        <Loader isLoading={isActivelyLoading || isNavigating} />
      </div>

      {page <= totalPages && !isLoading && (
        <PaginationWrapper
          page={page}
          totalPages={totalPages}
          onPageChange={handlePageChange}
          moviesToDisplay={moviesToDisplay}
        />
      )}
    </div>
  );
}
