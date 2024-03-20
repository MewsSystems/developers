import React, { useState, useEffect, useCallback } from 'react';
import { Container, Typography, CircularProgress } from '@mui/material';
import { useDispatch, useSelector } from 'react-redux';
import { Dispatch } from 'redux';
import { searchMovies } from '../store/actions/movies.actions';

import Search from './movieComponent/Search';
import MovieList from './movieComponent/MovieList';
import '../styles/MainView.css';


const MainView: React.FC = () => {
  const dispatch: Dispatch<any> = useDispatch();
  const [page, setPage] = useState(1);
  const [hasMore, setHasMore] = useState(true);
  const [searchText, setSearchText] = useState('');
  const [searchPerform, setSearchPerform] = useState(false);

  const { movies, loading, total_pages, error } = useSelector((state: any) => state.movies);

  const handleSearch = (searchText: string) => {
    if (searchText.trim() !== '') {
      setSearchText(searchText.trim());
      setPage(1);
      dispatch(searchMovies(searchText.trim(), page));
      setSearchPerform(true);
    } else {
      setSearchText('');
      setPage(1);
      dispatch(searchMovies('', 1));
      setSearchPerform(false);
    }
  };

  const loadMoreMovies = useCallback(async () => {
    if (loading || !hasMore) return;

    const nextPage = page + 1;
    setPage(nextPage);
    await dispatch(searchMovies(searchText, nextPage));

    // Check if next page exceeds total pages
    if (nextPage >= total_pages) {
        setHasMore(false); // No more movies to load
    }
}, [loading, hasMore, page, total_pages, dispatch, searchText]);

useEffect(() => {
  function handleScroll() {
      if (
          window.innerHeight + document.documentElement.scrollTop !==
          document.documentElement.offsetHeight
      ) {
          return;
      }
      loadMoreMovies();
  }

  window.addEventListener('scroll', handleScroll);
  return () => {
      window.removeEventListener('scroll', handleScroll);
  };
}, [loadMoreMovies]);

  const renderSpinner = loading && movies !== null && (
    <div className="spinner">
      <CircularProgress />
    </div>
  );

  const renderError = error && error.details && (
    <Typography variant="body1" color="error" align="center">
      {error.details}
    </Typography>
  );

  return (
    <>
      <Container className="container">
        <Typography variant="h1" component="h1" align="center" className="title">
          Movie Search
        </Typography>
        <Search onSearch={handleSearch}/>
        {/* Render MovieList only if movies are loaded */}
        {movies && <MovieList movies={movies} searchPerformed={searchPerform} />}
        {/* Render the spinner */}
        {renderSpinner}
        {/* Render the spinner */}
        {renderError}
      </Container>
    </>
  );
};

export default MainView;
