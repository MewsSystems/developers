import './searchInput.css';
import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const API_KEY = '03b8572954325680265531140190fd2a';

interface Movie {
  id: number;
  title: string;
  backdrop_path: string;
}

interface ApiResponse {
  results: Movie[];
  total_results: number;
  total_pages: number;
}

const SearchInput: React.FC = () => {
  const [query, setQuery] = useState<string>('');
  const [movies, setMovies] = useState<Movie[]>([]);
  const [totalResults, setTotalResults] = useState<number>(0);
  const [debouncedQuery, setDebouncedQuery] = useState<string>(query);
  const [page, setPage] = useState<number>(1);
  const [hasMore, setHasMore] = useState<boolean>(true);
  const [cache, setCache] = useState<{
    [key: string]: { results: Movie[]; total_pages: number };
  }>({});
  const navigate = useNavigate();

  useEffect(() => {
    document.body.classList.add('search-background');
    return () => {
      document.body.classList.remove('search-background');
    };
  }, []);

  useEffect(() => {
    const handler = setTimeout(() => {
      setDebouncedQuery(query);
      setPage(1); 
      setHasMore(true); 
    }, 400);
    return () => {
      clearTimeout(handler);
    };
  }, [query]);

  useEffect(() => {
    const searchMovies = async (searchQuery: string, pageNum: number) => {
      if (searchQuery.length > 2) {
        const cacheKey = `${searchQuery}-${pageNum}`;
        if (cache[cacheKey]) {
          const cachedData = cache[cacheKey];
          if (pageNum === 1) {
            setMovies(cachedData.results);
          } else {
            setMovies((prevMovies) => [...prevMovies, ...cachedData.results]);
          }
          setHasMore(pageNum < cachedData.total_pages);
        } else {
          try {
            const response = await axios.get<ApiResponse>(
              'https://api.themoviedb.org/3/search/movie',
              {
                params: {
                  api_key: API_KEY,
                  query: searchQuery,
                  page: pageNum,
                },
              },
            );
            if (pageNum === 1) {
              setMovies(response.data.results);
            } else {
              setMovies((prevMovies) => [
                ...prevMovies,
                ...response.data.results,
              ]);
            }
            setTotalResults(response.data.total_results);
            setHasMore(pageNum < response.data.total_pages);
            setCache((prevCache) => ({
              ...prevCache,
              [cacheKey]: {
                results: response.data.results,
                total_pages: response.data.total_pages,
              },
            }));
          } catch (error) {
            console.error('Error fetching movies:', error);
          }
        }
      } else {
        setMovies([]);
        setTotalResults(0);
        setHasMore(false);
      }
    };

    if (debouncedQuery) {
      searchMovies(debouncedQuery, page);
    }
  }, [debouncedQuery, page, cache]);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const searchQuery = event.target.value;
    setQuery(searchQuery);
    if (searchQuery === '') {
      setMovies([]);
      setTotalResults(0);
      setHasMore(false);
    }
  };

  const handleRowClick = (id: number) => {
    navigate(`/movieDetail/${id}`);
  };

  const loadMoreMovies = () => {
    setPage((prevPage) => prevPage + 1);
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
        <p className='total-results'>
          <strong>Total results: {totalResults}</strong>
        </p>
      </div>
      <div className="list">
        <ul>
          {movies.map((movie) => (
            <li key={movie.id} onClick={() => handleRowClick(movie.id)}>
              {movie.title}
              {movie.backdrop_path && (
                <img
                  src={`https://image.tmdb.org/t/p/w500${movie.backdrop_path}`}
                  alt={`${movie.title} backdrop`}
                  loading="lazy"
                />
              )}
            </li>
          ))}
        </ul>
      </div>
      {movies.length > 0 && hasMore && (
          <button onClick={loadMoreMovies} className="next">
            Next 20 movies...
          </button>
        )}
    </div>
  );
};

export default SearchInput;
