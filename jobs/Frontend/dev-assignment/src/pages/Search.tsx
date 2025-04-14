import React, { useState, useEffect } from 'react';
import { fetchMovies } from '../api';
import { Movie } from '../types';
import MovieCard from '../MovieCard';

export default function Search() {
  const [query, setQuery] = useState('');
  const [movies, setMovies] = useState<Movie[]>([]);
  const [page, setPage] = useState(1);

  useEffect(() => {
    const timeout = setTimeout(() => {
      if (query.trim()) {
        fetchMovies(query, 1).then(data => {
          setMovies(data.results);
          setPage(1);
        });
      }
    }, 500);
    return () => clearTimeout(timeout);
  }, [query]);

  const loadMore = () => {
    const next = page + 1;
    fetchMovies(query, next).then(data => {
      setMovies(prev => [...prev, ...data.results]);
      setPage(next);
    });
  };

  return (
    <div style={{ padding: 20 }}>
      <input
        placeholder="Search movies..."
        value={query}
        onChange={e => setQuery(e.target.value)}
        style={{ padding: 10, width: '100%', marginBottom: 20 }}
      />
      <div style={{ display: 'flex', flexWrap: 'wrap' }}>
        {movies.map(movie => <MovieCard key={movie.id} movie={movie} />)}
      </div>
      {movies.length > 0 && (
        <button onClick={loadMore} style={{ marginTop: 20 }}>Load More</button>
      )}
    </div>
  );
}