import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import SearchBar from '../Components/SearchBar';
import MovieList from '../Components/MovieList';
import LoadMoreButton from '../Components/LoadMoreButton';
import useMovieSearch from '../Components/useMovieSearch';
import './homePage.css';

const HomePage: React.FC = () => {
  const [query, setQuery] = useState<string>('');
  const [debouncedQuery, setDebouncedQuery] = useState<string>(query);
  const [page, setPage] = useState<number>(1);
  const navigate = useNavigate();

  const { movies, totalResults, hasMore } = useMovieSearch(debouncedQuery, page);

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
    }, 400);
    return () => {
      clearTimeout(handler);
    };
  }, [query]);

  const handleInputChange = (searchQuery: string) => {
    setQuery(searchQuery);
    if (searchQuery === '') {
      setPage(1);
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
        <SearchBar query={query} onQueryChange={handleInputChange} />
      </div>
      <div className="results">
        <p className="total-results">
          <strong>Total results: {totalResults}</strong>
        </p>
      </div>
      <div className="list">
        <MovieList movies={movies} onMovieClick={handleRowClick} />
      </div>
      {movies.length > 0 && hasMore && (
        <LoadMoreButton onLoadMore={loadMoreMovies} hasMore={hasMore} />
      )}
    </div>
  );
};

export default HomePage;
