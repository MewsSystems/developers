import './searchInput.css';
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const API_KEY = '03b8572954325680265531140190fd2a';

interface Movie {
  id: number;
  title: string;
}

interface ApiResponse {
  results: Movie[];
  total_results: number;
}

const SearchInput: React.FC = () => {
  const [query, setQuery] = useState<string>('');
  const [movies, setMovies] = useState<Movie[]>([]);
  const [totalResults, setTotalResults] = useState<number>(0);
  const [debouncedQuery, setDebouncedQuery] = useState<string>(query);
  const navigate = useNavigate();

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedQuery(query);
    }, 400);
    return () => {
      clearTimeout(handler);
    };
  }, [query]);

  useEffect(() => {
    const searchMovies = async (searchQuery: string) => {
      if (searchQuery.length > 2) {
        try {
          const response = await axios.get<ApiResponse>(
            'https://api.themoviedb.org/3/search/movie',
            {
              params: {
                api_key: API_KEY,
                query: searchQuery,
              },
            },
          );
          setMovies(response.data.results);
          setTotalResults(response.data.total_results);
        } catch (error) {
          console.error('Error fetching movies:', error);
        }
      } else {
        setMovies([]);
        setTotalResults(0);
      }
    };

    if (debouncedQuery) {
      searchMovies(debouncedQuery);
    }
  }, [debouncedQuery]);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const searchQuery = event.target.value;
    setQuery(searchQuery);
    if (searchQuery === '') {
      setMovies([]);
      setTotalResults(0);
    }
  };

  const handleRowClick = (id: number) => {
    navigate(`/movieDetail/${id}`);
  };

  return (
    <div className="search">
      <div className="title">
        <h1>Find your movie</h1>
      </div>
      <div className="input">
        <input
          type="search"
          id="search"
          placeholder="Search for a movie..."
          value={query}
          onChange={handleInputChange}
        />
      </div>
      <div className="results">
        <p><strong>Total results: {totalResults}</strong></p>
      </div>
      <div className='list'>
        <ul>
          {movies.map((movie) => (
            <li key={movie.id} onClick={() => handleRowClick(movie.id)}>
              {movie.title}
            </li>
          ))}
        </ul>
      </div>
    </div>
  );
};

export default SearchInput;
