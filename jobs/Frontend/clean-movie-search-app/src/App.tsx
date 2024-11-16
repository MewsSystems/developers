import React, { useState, useCallback } from 'react';
import { searchMovies } from './api/movieApi';
import { Movie } from './api';
import { SearchBar } from './components/SearchBar/SearchBar';
import { MovieList } from './components/MovieList/MovieList';
import { useSearchDebounce } from './hooks/useSearchDebounce';
import styled from 'styled-components';

const Container = styled.div`
  max-width: 1200px;
  margin: 0 auto;
  padding: 20px;
`;

const Title = styled.h1`
  font-size: 2rem;
  color: #333;
  margin-bottom: 24px;
`;

function App() {
  const [query, setQuery] = useState('');
  const [movies, setMovies] = useState<Movie[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>('');

  const debouncedQuery = useSearchDebounce(query);

  const handleSearch = useCallback(async (searchQuery: string) => {
    if (!searchQuery.trim()) {
      // TODO - Better handle empty search query
      setMovies([]);
      return;
    }

    try {
      setLoading(true);
      setError('');
      const response = await searchMovies(searchQuery);
      setMovies(response.results);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'An error occurred retriving movies');
      setMovies([]);
    } finally {
      setLoading(false);
    }
  }, []);

  React.useEffect(() => {
    handleSearch(debouncedQuery);
  }, [debouncedQuery, handleSearch]);

  const handleMovieClick = (movieId: number) => {
    console.log('Id of the selected movie:', movieId);
    // Navigation to movie details to be introduced
  };

  return (
    <Container>
      <Title>What to watch</Title>
      <SearchBar value={query} onChange={setQuery}/>
      {loading && <div>Finding movies...</div>}
      {error && <div>Error: {error}</div>}
      {!loading && !error && <MovieList movies={movies} onMovieClick={handleMovieClick} />}
    </Container>
  );
}

export default App;