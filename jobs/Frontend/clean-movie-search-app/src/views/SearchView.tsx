import React from 'react';
import { useNavigate } from 'react-router-dom';
import { SearchBar, MovieList } from '../components';
import { useSearchDebounce } from '../hooks/useSearchDebounce';
import { useMovieContext } from '../context/MovieContext';
import styled from 'styled-components';

const LoadMoreButton = styled.button`
  display: block;
  margin: ${({ theme }) => `${theme.spacing.xl} auto`};
  padding: ${({ theme }) => `${theme.spacing.md} ${theme.spacing.xl}`};
  background-color: ${({ theme }) => theme.colors.button.background};
  color: ${({ theme }) => theme.colors.button.text};
  border: none;
  border-radius: ${({ theme }) => theme.borderRadius.md};
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.2s;

  &:hover {
    background-color: ${({ theme }) => theme.colors.button.hover};
  }

  &:disabled {
    background-color: ${({ theme }) => theme.colors.text.secondary};
    cursor: not-allowed;
  }
`;

const LoadingIndicator = styled.div`
  text-align: center;
  padding: ${({ theme }) => theme.spacing.lg};
  color: ${({ theme }) => theme.colors.text.secondary};
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
