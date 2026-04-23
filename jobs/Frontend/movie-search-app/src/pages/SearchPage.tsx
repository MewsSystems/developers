import React, { useState } from 'react';
import SearchBar from '../components/SearchBar';
import MovieList from '../components/MovieList';
import { searchMovies } from '../api';
import { Movie } from '../types';

const SearchPage: React.FC = () => {
  const [movies, setMovies] = useState<Movie[]>([]);
  const [page, setPage] = useState(1);

  const handleSearch = async (query: string) => {
    const data = await searchMovies(query, page);
    setMovies(data.results);
  };

  return (
    <div>
      <SearchBar onSearch={handleSearch} />
      <MovieList movies={movies} />
    </div>
  );
};

export default SearchPage;