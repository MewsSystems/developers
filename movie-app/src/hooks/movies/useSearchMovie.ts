import { fetchAPI, movieDbApiKey } from "../../api/config.ts";
import { keepPreviousData, useQuery } from "@tanstack/react-query";
import { ChangeEvent, useRef, useState } from "react";
import { debounce } from "@mui/material";
import { MoviesSearchApiResponse } from "./types.ts";

export const useSearchMovie = () => {
  const searchRef = useRef<HTMLInputElement>(null);

  const [currentSearch, setCurrentSearch] = useState<string>("");
  const [page, setPage] = useState<number>(1);

  const handleSearch = debounce((search: string) => {
    setCurrentSearch(search);
    setPage(1); // Reset page to 1 when performing a new search
  }, 500);

  const handlePageChange = (_: ChangeEvent<unknown>, value: number) => {
    setPage(value);
    window.scroll({ top: 0, left: 0, behavior: "smooth" });
  };

  const handleClear = () => {
    if (searchRef.current) searchRef.current.value = "";
    setCurrentSearch("");
    setPage(1);
  };

  const { data, isLoading, isError } = useQuery({
    queryKey: ["searchMovie", currentSearch, page],
    queryFn: () =>
      fetchAPI(
        `search/movie?api_key=${movieDbApiKey}&query=${currentSearch}&page=${page}`,
      ),
    placeholderData: keepPreviousData,
    enabled: currentSearch.length > 2,
  });

  const {
    page: currentPage,
    results,
    total_pages: totalPages,
    total_results: totalResults,
  } = (data as MoviesSearchApiResponse) || {};

  return {
    page,
    currentPage,
    results,
    totalPages,
    totalResults,
    handleSearch,
    handlePageChange,
    handleClear,
    data,
    currentSearch,
    searchRef,
    isError,
    isLoading,
  };
};
