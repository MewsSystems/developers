import React, { useState } from 'react';
import axios from 'axios';

const API_KEY = '03b8572954325680265531140190fd2a';

interface Movie {
  id: number;
  title: string;
}

const SearchInput: React.FC = () => {
  const [query, setQuery] = useState<string>('');
  const [movies, setMovies] = useState<Movie[]>([]);

  const searchMovies = async (searchQuery: string) => {
    if (searchQuery.length > 2) {
      try {
        const response = await axios.get('https://api.themoviedb.org/3/search/movie', {
          params: {
            api_key: API_KEY,
            query: searchQuery
          }
        });
        setMovies(response.data.results);
      } catch (error) {
        console.error('Error fetching movies:', error);
      }
    } else {
      setMovies([]);
    }
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const searchQuery = event.target.value;
    setQuery(searchQuery);
    searchMovies(searchQuery);
  };

  return (
    <>
      <h3>Find your movie</h3>
      <input
        type="search"
        id="search"
        placeholder="Search for a movie..."
        value={query}
        onChange={handleInputChange}
      />
      <ul>
        {movies.map((movie) => (
          <li key={movie.id}>{movie.title}</li>
        ))}
      </ul>
    </>
  );
};

export default SearchInput;
