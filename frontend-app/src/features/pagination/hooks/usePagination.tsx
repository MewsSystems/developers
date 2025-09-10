import { useMemo } from 'react';

interface UsePaginationProps {
  currentPage: number;
  totalPages: number;
}

interface UsePaginationReturn {
  pageNumbers: Array<number | string>;
  canGoPrevious: boolean;
  canGoNext: boolean;
  isVisible: boolean;
}

/**
 * Hook for managing pagination logic
 * Calculates page numbers for display and navigation button states
 */
export const usePagination = ({ currentPage, totalPages }: UsePaginationProps): UsePaginationReturn => {
  /**
   * Function to create an array of pages for display
   */
  const pageNumbers = useMemo(() => {
    const pages: Array<number | string> = [];
    const maxPagesToShow = 5; // Maximum number of page buttons to display

    // If total pages is less than or equal to maximum pages to show
    if (totalPages <= maxPagesToShow) {
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      // Always show the first page
      pages.push(1);

      // Calculate range of pages around current page
      let startPage = Math.max(2, currentPage - 1);
      let endPage = Math.min(totalPages - 1, currentPage + 1);

      // If current page is close to the beginning
      if (currentPage <= 3) {
        endPage = 4;
      }

      // If current page is close to the end
      if (currentPage >= totalPages - 2) {
        startPage = totalPages - 3;
      }

      // Add ellipsis after first page if needed
      if (startPage > 2) {
        pages.push('...');
      }

      // Add pages from range
      for (let i = startPage; i <= endPage; i++) {
        pages.push(i);
      }

      // Add ellipsis before last page if needed
      if (endPage < totalPages - 1) {
        pages.push('...');
      }

      // Always show the last page
      pages.push(totalPages);
    }

    return pages;
  }, [currentPage, totalPages]);

  /**
   * Determine navigation button states
   */
  const canGoPrevious = currentPage > 1;
  const canGoNext = currentPage < totalPages;
  const isVisible = totalPages > 1;

  return {
    pageNumbers,
    canGoPrevious,
    canGoNext,
    isVisible,
  };
};