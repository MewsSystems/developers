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
      {currentPage !== 1 ? <PageButton onClick={() => onPageClick(1)}>{1}</PageButton> : <span />}
      <PageButton onClick={onPrevClick}>-</PageButton>
      <PageButton onClick={noop}>{currentPage}</PageButton>
      <PageButton onClick={onNextClick}>+</PageButton>
      <PageButton onClick={() => onPageClick(totalPages)}>{totalPages}</PageButton>
    </PaginationWrapper>
  );
};
