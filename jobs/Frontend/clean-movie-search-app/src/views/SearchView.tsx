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

  // Custom hook to debounce the search query
  const debouncedQuery = useSearchDebounce(query);

  // Effect to handle search when debounced query changes
  React.useEffect(() => {
    if (debouncedQuery.trim()) {
      // We reset page to 1 when a new search is made
      handleSearch(debouncedQuery, 1);
    }
    // We launch the actual search when the debounced query "changes"
    // This change occurs after the specified delay in the hoook and the answer is returned
  }, [debouncedQuery, handleSearch]);

  const handleMovieClick = (movieId: number) => {
    navigate(`/movie/${movieId}`);
  };

  return (
    <>
      {/* Once there is a change to the search query, it updates the context value, which in turn updates the query which triggers the debouncer */}
      <SearchBar value={query} onChange={setQuery} />
      {loading && <LoadingIndicator>Finding movies...</LoadingIndicator>}
      {error && <div>Error: {error}</div>}
      <MovieList movies={movies} onMovieClick={handleMovieClick} />
      {paginationLoading && (
        <LoadingIndicator>Finding more movies...</LoadingIndicator>
      )}
      {!paginationLoading && movies.length > 0 && page < totalPages && (
        <LoadMoreButton onClick={loadMore}>
          Load More Movies
        </LoadMoreButton>
      )}
    </>
  );
};
