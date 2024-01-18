import Flex from '@/components/Flex';
import React from 'react';
import styled from 'styled-components';
import { PaginationButton } from '@/components/Pagination/PaginationButton';

export interface PaginationProps {
  totalPages: number;
  currentPage: number;
  onNextPage: () => void;
  onPrevPage: () => void;
  onSetPage: (page: number) => void;
}

export interface PaginationButtonProps {
  active?: boolean;
}

const PaginationWrapper = styled(Flex)`
  ${PaginationButton}{
    &:first-of-type {
      border-top-left-radius: 8px;   
      border-bottom-left-radius: 8px;   
    }
    &:last-of-type {
      border-top-right-radius: 8px;   
      border-bottom-right-radius: 8px;   
    }
    + ${PaginationButton}{
      border-left: none;
    }
  }
  
`

PaginationButton.defaultProps = {
  p: '6px 12px',
}

const Pagination = ({totalPages, currentPage, onNextPage, onPrevPage, onSetPage}: PaginationProps) => {
  const pages = new Array(totalPages).fill(null);
  return (
    <PaginationWrapper flexWrap="wrap">
      <PaginationButton onClick={() => onSetPage(1)}>
        First
      </PaginationButton>
      <PaginationButton onClick={onPrevPage}>
        Prev
      </PaginationButton>
      {
        pages.map((_, index) => {
          const pageNumber = index + 1;
          const isActive = pageNumber === currentPage
          return (
            <PaginationButton
              key={pageNumber}
              active={isActive}
              data-testid={`page-${pageNumber}-button`}
              title={isActive ? `Current Page` : undefined}
              onClick={() => onSetPage(pageNumber)}
            >
              {pageNumber}
          </PaginationButton>
          );
        })
      }
      <PaginationButton onClick={onNextPage}>
        Next
      </PaginationButton>
      <PaginationButton onClick={() => onSetPage(totalPages)}>
        Last
      </PaginationButton>
    </PaginationWrapper>
  )
}

export default Pagination;
