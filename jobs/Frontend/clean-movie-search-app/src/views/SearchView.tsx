import React from 'react';
import { useNavigate } from 'react-router-dom';
import { SearchBar } from '../components/SearchBar/SearchBar';
import { MovieList } from '../components/MovieList/MovieList';
import { useSearchDebounce } from '../hooks/useSearchDebounce';
import { useMovieContext } from '../context/MovieContext';
import styled from 'styled-components';

const LoadMoreButton = styled.button`
  display: block;
  margin: 24px auto;
  padding: 12px 24px;
  background-color: #007bff;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;

  &:hover {
    background-color: #0056b3;
  }

  &:disabled {
    background-color: #cccccc;
    cursor: not-allowed;
  }
`;

const LoadingIndicator = styled.div`
  text-align: center;
  padding: 20px;
`;

export const SearchView: React.FC = () => {
  const navigate = useNavigate();
  const {
    query,
    setQuery,
    movies,
    loading,
    paginationLoading,
    error,
    page,
    totalPages,
    handleSearch,
    loadMore,
  } = useMovieContext();

  const debouncedQuery = useSearchDebounce(query);

  React.useEffect(() => {
    if (debouncedQuery.trim()) {
      handleSearch(debouncedQuery, 1);
    }
  }, [debouncedQuery, handleSearch]);

  const handleMovieClick = (movieId: number) => {
    navigate(`/movie/${movieId}`);
  };

  return (
    <>
      <SearchBar value={query} onChange={setQuery} />
      {loading && <LoadingIndicator>Finding movies...</LoadingIndicator>}
      {error && <div>Error: {error}</div>}
      <MovieList movies={movies} onMovieClick={handleMovieClick} />
      {paginationLoading && (
        <LoadingIndicator>Loading more movies...</LoadingIndicator>
      )}
      {movies.length > 0 && page < totalPages && (
        <LoadMoreButton onClick={loadMore} disabled={paginationLoading}>
          Load More Movies
        </LoadMoreButton>
      )}
    </>
  );
};
