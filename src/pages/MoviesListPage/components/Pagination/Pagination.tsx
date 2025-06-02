import {memo, useCallback, useMemo} from 'react';

import {useGetPages} from './hooks/useGetPages';
import {NavigationButton, PaginationWrapper} from './styled';

type PaginationProps = {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
};

function Pagination({currentPage, totalPages, onPageChange}: PaginationProps) {
  const pages = useGetPages({currentPage, totalPages});

  const isFirstPage = useMemo(() => currentPage === 1, [currentPage]);
  const isLastPage = useMemo(() => currentPage === totalPages, [currentPage, totalPages]);

  const onPrevClick = useCallback(() => {
    onPageChange(currentPage - 1);
  }, [currentPage, onPageChange]);

  const onNextClick = useCallback(() => {
    onPageChange(currentPage + 1);
  }, [currentPage, onPageChange]);

  const onPageNumberClick = useCallback(
    (pageNumber: number) => {
      onPageChange(pageNumber);
    },
    [onPageChange],
  );

  if (totalPages <= 1) return null;

  return (
    <PaginationWrapper>
      <NavigationButton onClick={onPrevClick} disabled={isFirstPage}>
        {'<'}
      </NavigationButton>

      {pages.map((pageNumber) => (
        <NavigationButton
          key={pageNumber}
          onClick={() => onPageNumberClick(pageNumber)}
          disabled={pageNumber === currentPage}
          $active={pageNumber === currentPage}
        >
          {pageNumber}
        </NavigationButton>
      ))}

      <NavigationButton onClick={onNextClick} disabled={isLastPage}>
        {'>'}
      </NavigationButton>
    </PaginationWrapper>
  );
}

Pagination.displayName = 'Pagination';

export default memo(Pagination);
