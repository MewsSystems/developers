import React, { useState } from 'react';
import Grid from '@mui/material/Grid';
import Pagination from '../../components/pagination/pagination';
import Header from '../../components/header/header';
import { PaginationInfo } from '../../interfaces/pagination.interface';
import { MovieList } from '../../interfaces/movie.interface';
import MovieListContainer from './movie-list-container/movies-list-container';
import SearchContainer from './search-container/search-container';
import debounce from '@mui/utils/debounce/debounce';

const MovieSearch = () => {
  const initPagination = { page: 1, count: null };

  const [pagination, setPagination] = useState<PaginationInfo>(initPagination);
  const [searchTerm, setSearchTerm] = useState<string | null>(null);

  const handleOnSearch = debounce((searchTerm: string) => {
    setPagination(initPagination);
    setSearchTerm(searchTerm === '' ? null : searchTerm);
  }, 400);

  const handleOnPageChange = (page: number) => {
    setPagination((currentPagination: PaginationInfo) => ({
      ...currentPagination,
      page,
    }));
  };

  const handleFetchMoviesSuccess = (movies: MovieList) => {
    setPagination((currentPagination) => ({
      ...currentPagination,
      count: movies.total_pages,
    }));
  };

  const handleFetchMoviesError = () => {
    setPagination(initPagination);
    setSearchTerm(null);
  };

  return (
    <>
      <Header />
      <Grid container justifyContent="center" alignItems="center">
        <SearchContainer handleOnSearch={handleOnSearch} />
        <MovieListContainer
          searchTerm={searchTerm}
          page={pagination.page}
          onFetchMoviesSuccess={handleFetchMoviesSuccess}
          onFetchMoviesError={handleFetchMoviesError}
        />
        {searchTerm && pagination.count && (
          <Grid item className="p-t-16 p-b-16">
            <Pagination
              page={pagination.page}
              count={pagination.count}
              onPageChange={handleOnPageChange}
            />
          </Grid>
        )}
      </Grid>
    </>
  );
};

export default MovieSearch;
