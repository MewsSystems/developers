import React, { FC } from 'react';
import { PageButton, PaginationWrapper } from './styles';
import { noop } from 'lodash';

export const Pagination: FC<{
  totalPages: number;
  currentPage: number;
  onPageClick: (page: number) => void;
  onPrevClick: () => void;
  onNextClick: () => void;
}> = ({ onPageClick, onPrevClick, onNextClick, totalPages, currentPage }) => {
  return (
    <PaginationWrapper>
      <PageButton onClick={() => onPageClick(1)}>{`First Page`}</PageButton>
      {currentPage !== 1 && <PageButton onClick={onPrevClick}>-</PageButton>}
      <PageButton onClick={noop}>{currentPage}</PageButton>
      {currentPage !== totalPages && <PageButton onClick={onNextClick}>+</PageButton>}
      <PageButton onClick={() => onPageClick(totalPages)}>{`Last Page (${totalPages})`}</PageButton>
    </PaginationWrapper>
  );
};
