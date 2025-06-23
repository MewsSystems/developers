import React, {  useEffect, useRef } from 'react';
import { useNavigate } from 'react-router-dom';
import SearchBar from '../Components/SearchBar';
import MovieList from '../Components/MovieList';
import LoadMoreButton from '../Components/LoadMoreButton';
import useMovieSearch from '../Components/useMovieSearch';
import './homePage.css';

const HomePage: React.FC = () => {
  const {
    searchInput,
    page,
    movies,
    totalResults,
    hasMore,
    setSearchInput,
    setPage,
    searchMovies,
  } = useMovieSearch('', 1);

  const navigate = useNavigate();
  const delayTimeout = useRef<ReturnType<typeof setTimeout> | null>(null);
  useEffect(() => {
    document.body.classList.add('search-background');
    return () => {
      document.body.classList.remove('search-background');
    };
  }, []);

  const handleInputChange = (newSearchInput: string) => {
    setSearchInput(newSearchInput);
    setPage(1);

    if (delayTimeout.current) {
      clearTimeout(delayTimeout.current);
    }

    delayTimeout.current = setTimeout(() => {
      searchMovies(newSearchInput, 1);
    }, 400);
  };

  const handleClick = (id: number) => {
    navigate(`/movieDetail/${id}`);
  };

  const loadMoreMovies = () => {
    const nextPage = page + 1;
    setPage(nextPage);
    searchMovies(searchInput, nextPage);
  };

  return (
    <div className="search">
      <div className="title">
        <h1>Find your movie</h1>
      </div>
      <div className="input">
        <SearchBar searchInput={searchInput} onSearchInputChange={handleInputChange} />
      </div>
      {totalResults > 0 && (
        <div className="results">
          <p className="total-results">
            <strong>Total results: {totalResults}</strong>
          </p>
        </div>
      )}
      <div className="list">
        <MovieList movies={movies} onMovieClick={handleClick} />
      </div>
      {movies.length > 0 && hasMore && (
        <LoadMoreButton onLoadMore={loadMoreMovies} hasMore={hasMore} />
      )}
    </div>
  );
};

export default HomePage;
