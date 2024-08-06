import React, { useState, useEffect, useCallback } from "react";
import styled from "styled-components";
import { useSearchParams } from "react-router-dom";
import { searchMovies } from "../api/tmdb";
import { Movie, MoviesResponse } from "../types/MovieInterfaces";
import SearchInput from "../components/SearchInput";
import MovieList from "../components/MovieList";
import Pagination from "../components/Pagination";
import { debounce } from "lodash";

const SearchContainer = styled.div`
  padding: 2rem;
`;

const SearchView: React.FC = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const queryFromUrl = searchParams.get("query") || "";
  const pageFromUrl = parseInt(searchParams.get("page") || "1", 10);
  const [query, setQuery] = useState(queryFromUrl);
  const [movies, setMovies] = useState<Movie[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [page, setPage] = useState(pageFromUrl);
  const [totalPages, setTotalPages] = useState(1);
  const [searchInitiated, setSearchInitiated] = useState(false);

  const handleSearch = useCallback(async () => {
    if (!query) {
      setMovies([]);
      return;
    }
    setError(null);
    try {
      const data: MoviesResponse = await searchMovies(query, page);
      setMovies(data.results);
      setTotalPages(data.total_pages);
      setSearchParams({ query, page: page.toString() });
      setSearchInitiated(true);
    } catch (err) {
      setError("Error fetching movies");
    }
  }, [query, page, setSearchParams]);

  useEffect(() => {
    const debouncedSearch = debounce(handleSearch, 2000);
    debouncedSearch();

    return () => debouncedSearch.cancel();
  }, [query, handleSearch]);

  useEffect(() => {
    if (query) {
      handleSearch();
    }
  }, [query, page, handleSearch]);

  const handleNextPage = () => {
    if (page < totalPages) {
      setPage((prevPage) => {
        const newPage = prevPage + 1;
        setSearchParams({ query, page: newPage.toString() });
        return newPage;
      });
    }
  };

  const handlePrevPage = () => {
    if (page > 1) {
      setPage((prevPage) => {
        const newPage = prevPage - 1;
        setSearchParams({ query, page: newPage.toString() });
        return newPage;
      });
    }
  };

  return (
    <SearchContainer>
      <SearchInput value={query} onChange={(e) => setQuery(e.target.value)} />
      {error && <p>{error}</p>}
      {searchInitiated && <p>Movies found: {movies.length}</p>}
      <MovieList movies={movies} />
      <Pagination
        page={page}
        totalPages={totalPages}
        onNextPage={handleNextPage}
        onPrevPage={handlePrevPage}
      />
    </SearchContainer>
  );
};

export default SearchView;
