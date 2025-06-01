import {useMemo} from 'react';

type UseGetPagesProps = {
  currentPage: number;
  totalPages: number;
};

export const useGetPages = ({currentPage, totalPages}: UseGetPagesProps): number[] => {
  return useMemo(() => {
    const MAX_PAGES_COUNT = 5;
    const pageNumbers = [];
    let startPage: number;
    let endPage: number;

    if (totalPages <= MAX_PAGES_COUNT) {
      startPage = 1;
      endPage = totalPages;
    } else {
      const middlePosition = Math.floor(MAX_PAGES_COUNT / 2);

      if (currentPage <= middlePosition + 1) {
        startPage = 1;
        endPage = MAX_PAGES_COUNT;
      } else if (currentPage + middlePosition >= totalPages) {
        startPage = totalPages - MAX_PAGES_COUNT + 1;
        endPage = totalPages;
      } else {
        startPage = currentPage - middlePosition;
        endPage = currentPage + middlePosition;
      }
    }

    for (let i = startPage; i <= endPage; i++) {
      pageNumbers.push(i);
    }

    return pageNumbers;
  }, [currentPage, totalPages]);
};
