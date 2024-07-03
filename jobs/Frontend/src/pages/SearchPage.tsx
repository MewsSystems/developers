import React, { useState, useEffect, memo } from 'react';
import SearchBar from '../components/SearchBar';
import MovieCard from '../components/MovieCard';
import styled from 'styled-components';
import { useMovies } from '../hooks/useMovies';

const Container = styled.div`
  padding: 16px;
`;

const SearchPage: React.FC = memo(() => {
  console.log('load the search page')
  const { movies, searchMovies, resetMovies, loading, error } = useMovies();
  const [page, setPage] = useState(1);
  const [query, setQuery] = useState('');

  useEffect(() => {
    if (query) {
      console.log('searchMovies:', query)
      searchMovies(query, page);
    }
  }, [query, page, searchMovies]);

  const handleSearch = (searchQuery: string) => {
    console.log('searching for:', searchQuery)
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
      <div>
        {movies.map((movie) => (
          <MovieCard key={movie.id} movie={movie} />
        ))}
      </div>
      <button onClick={loadMore}>Load More</button>
    </Container>
  );
});

export default SearchPage;
