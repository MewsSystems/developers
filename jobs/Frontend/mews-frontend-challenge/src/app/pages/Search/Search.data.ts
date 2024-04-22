import { MovieResult, tmdb } from "@/app/services/tmdb";
import { useMemo } from "react";
import { useLocation } from "react-router-dom";

export type SearchPageFilters = {
  q: string;
  page: number;
};

export type MovieSearchResponse = {
  movies: MovieResult[];
  pagination: PaginationData;
};

export type PaginationData = {
  page: number;
  totalPages: number;
  totalResults: number;
};

export async function getMovies(params: SearchPageFilters) {
  const searchQuery = params?.q;
  const pages = params?.page || 1;

  const movieData = await tmdb.search.movies({
    query: searchQuery,
    page: pages,
  });

  return {
    pagination: {
      page: movieData.page || emptyResult.pagination.page,
      totalPages: movieData.total_pages || emptyResult.pagination.totalPages,
      totalResults:
        movieData.total_results || emptyResult.pagination.totalResults,
    },
    movies: movieData.results || emptyResult.movies,
  };
}

export const emptyResult = {
  movies: [],
  pagination: { page: 1, totalPages: 1, totalResults: 0 },
};

export function useSearchFilters() {
  const { search } = useLocation();
  const queryParams = useMemo(() => new URLSearchParams(search), [search]);

  return {
    q: queryParams.get("q") || "",
    page: Number(queryParams.get("page")) || 1,
  };
}
