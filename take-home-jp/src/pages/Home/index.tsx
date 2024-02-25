import React, { useState } from 'react';

import Input from '../../components/Input';
import ErrorLabel from '../../components/ErrorLabel';
import Pagination from '../../components/Pagination';
import MovieCard from '../../components/MovieCard';
import IntroScreen from '../../components/IntroScreen';
import useSearchMovies from '../../api/useSearchMovies';
import NoResultsSceen from '../../components/NoResultsScreen';
import { Movie } from '../../components/MovieCard/types';
import Container from '@mui/material/Container';
import Grid from '@mui/material/Grid';
import CircularProgress from '@mui/material/CircularProgress';
import Box from '@mui/material/Box';

const Homepage = () => {
  const {
    data,
    isLoading,
    error,
    search: searchMoviesByText,
    totalPages,
  } = useSearchMovies();
  const [hasSearched, setHasSearched] = useState<boolean>(false);
  const [searchQuery, setSearchQuery] = useState<string>('');
  const [pageNumber, setPageNumber] = useState<number>(1);
  const handleSearchInput = (text: string) => {
    setPageNumber(1);
    // Text is empty or search bar is cleared, no need to make requests
    if (!text) {
      setHasSearched(false);
      return;
    }
    setHasSearched(true);
    setSearchQuery(text);
    searchMoviesByText(text);
  };

  const handlePageChange = (selectedPageNumber: number) => {
    window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
    setPageNumber(selectedPageNumber);
    searchMoviesByText(searchQuery, selectedPageNumber);
  };

  return (
    <Container>
      <Box py={2}>
        <Input onChange={handleSearchInput} />
      </Box>
      {error && <ErrorLabel errorMessage={error} />}
      {isLoading && <CircularProgress size="large" color="secondary" />}
      {hasSearched === false ? (
        <IntroScreen />
      ) : data.length === 0 ? (
        <NoResultsSceen />
      ) : (
        <>
          <Grid container spacing={2}>
            {data.map((movie: Movie) => {
              return (
                <Grid key={movie.id} item xs={4}>
                  <MovieCard movie={movie} />
                </Grid>
              );
            })}
          </Grid>
          <Pagination
            currentPage={pageNumber}
            totalPages={totalPages}
            onChange={handlePageChange}
          />
        </>
      )}
    </Container>
  );
};

export default Homepage;
