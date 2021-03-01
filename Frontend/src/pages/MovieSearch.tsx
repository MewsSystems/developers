import React, { useCallback } from 'react';
import SearchInput from '../components/SearchInput';
import { useMovieSearch } from '../hooks';
import SearchResults from '../components/SearchResults';
import Layout from '../components/Layout';
import EmptyState from '../components/EmptyState';

function MovieSearch() {
  const { query, searchMovies } = useMovieSearch();

  const setPage = useCallback((newPage) => searchMovies(query, newPage), [
    query,
    searchMovies,
  ]);

  return (
    <Layout
      header={
        <SearchInput
          placeholderText="Search for movies..."
          value={query}
          onChange={searchMovies}
        />
      }
    >
      {query ? (
        <SearchResults onPageClick={setPage} />
      ) : (
        <EmptyState title="Movie Search">
          Start typing on the search box above to find a movie.
        </EmptyState>
      )}
    </Layout>
  );
}

export default MovieSearch;
