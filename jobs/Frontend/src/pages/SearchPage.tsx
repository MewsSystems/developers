import React, { useState, useEffect, memo } from "react";
import SearchBar from "../components/SearchBar";
import MovieCardGrid from "../components/MovieCardGrid";
import styled from "styled-components";
import { useMovies } from "../hooks/useMovies";
import Button from "../components/Button";

const Container = styled.div`
  padding: 16px;
`;

const CenteredText = styled.div`
  text-align: center;
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
      {loading && <div>Loading...</div>}
      {error && <div>Error: {error}</div>}
      <MovieCardGrid movies={movies} />
      <CenteredText>
        <Button onClick={loadMore}>Load More</Button>
      </CenteredText>
    </Container>
  );
});

export default SearchPage;
