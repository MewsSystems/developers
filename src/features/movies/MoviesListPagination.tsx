import {Grid, Pagination} from '@mui/material';
import React from 'react';

interface MoviesListPaginationProps {
  page: number;
  totalPages: number;
  setPage: (page: number) => void;
}

export function MoviesListPagination(props: MoviesListPaginationProps) {
  const {page, totalPages, setPage} = props;
  const handleChange = (event: any, value: number) => {
    setPage(value);
  };
  return (
    <Grid item mb={2}>
      <Pagination count={totalPages} page={page} onChange={handleChange} shape="rounded" />
    </Grid>
  );
}
