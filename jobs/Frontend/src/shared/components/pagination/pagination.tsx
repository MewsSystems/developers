import React, { ChangeEvent } from 'react';
import MuiPagination from '@mui/material/Pagination';
import { PaginationProps } from './pagination.interface';

const Pagination = ({ count, page, onPageChange }: PaginationProps) => {
  const handlePageChange = (_: ChangeEvent<unknown>, page: number) => {
    onPageChange(page);
  };

  return (
    <MuiPagination page={page} count={count} onChange={handlePageChange} />
  );
};

export default Pagination;
