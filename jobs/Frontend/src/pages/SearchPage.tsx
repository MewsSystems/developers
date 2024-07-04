import React, { useState, useEffect, memo } from "react";
import styled from "styled-components";
import SearchBar from "../components/SearchBar";
import MovieCardGrid from "../components/MovieCardGrid";
import Button from "../components/Button";
import LoadingSpinner from "../components/LoadingSpinner";
import PageContainer from "../components/PageContainer";
import { useMovies } from "../hooks/useMovies";

const CenteredContent = styled.div`
  text-align: center;
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
    <PageContainer>
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
    </PageContainer>
  );
});

export default SearchPage;
