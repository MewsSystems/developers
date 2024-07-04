import React, { useState, useEffect, memo } from "react";
import SearchBar from "../components/SearchBar";
import MovieCardGrid from "../components/MovieCardGrid";
import styled from "styled-components";
import { useMovies } from "../hooks/useMovies";
import Button from "../components/Button";

const Container = styled.div`
  padding: 16px;
`;

const CenteredContent = styled.div`
  text-align: center;
`;

const LoadingSpinner = styled.div`
  border: 16px solid #f3f3f3;
  border-top: 16px solid #3498db;
  border-radius: 50%;
  width: 40px;
  height: 40px;
  animation: spin 2s linear infinite;
  margin: 0 auto;
  margin-top: 16px;
  margin-bottom: 16px;

  @keyframes spin {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
`;

const MessageEmptyResult = styled.div`
  text-align: center;
  margin-top: 16px;
`;

const SearchPage: React.FC = memo(() => {
  const { movies, searchMovies, resetMovies, loading, error } = useMovies();
  const [page, setPage] = useState(1);
  const [query, setQuery] = useState("");

  useEffect(() => {
    if (query) {
      searchMovies(query, page);
    }
  }, [query, page, searchMovies]);

  const handleSearch = (searchQuery: string) => {
    setQuery(searchQuery);
    resetMovies();
    setPage(1);
  };

  const loadMore = () => setPage((prevPage) => prevPage + 1);

  return (
    <Container>
      <SearchBar onSearch={handleSearch} />
      {error && <div>Error: {error}</div>}
      <MovieCardGrid movies={movies} />
      {loading && <LoadingSpinner />}
      {movies.length > 0 ? (
        <CenteredContent>
          <Button onClick={loadMore}>Load More</Button>
        </CenteredContent>
      ) : (
        <MessageEmptyResult>
          {query.length > 0
            ? "No results found."
            : "Please, type a movie title to start!"}
        </MessageEmptyResult>
      )}
    </Container>
  );
});

export default SearchPage;
