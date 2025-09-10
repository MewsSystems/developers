import React from 'react';
import { usePagination } from '../hooks/usePagination';
import {
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
  Pagination as ShadcnPagination,
} from '@/shared/ui/pagination';
import { cn } from '@/shared/lib/utils';

/**
 * Props interface for Pagination component
 */
interface PaginationProps {
  /** Current page */
  currentPage: number;
  /** Total number of pages */
  totalPages: number;
  /** Callback function for page change */
  onPageChange: (page: number) => void;
  /** Loading state */
  isLoading?: boolean;
  /** Additional CSS classes */
  className?: string;
}

/**
 * Pagination component using shadcn UI
 * Provides page navigation with Previous/Next buttons and page numbers
 */
export const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  onPageChange,
  isLoading = false,
  className,
}) => {
  const { pageNumbers, canGoPrevious, canGoNext, isVisible } = usePagination({
    currentPage,
    totalPages,
  });

  /**
   * Handler for page number click
   */
  const handlePageClick = (page: number | string) => {
    if (typeof page === 'number' && page !== currentPage && !isLoading) {
      onPageChange(page);
    }
  };

  /**
   * Handler for "Previous" button click
   */
  const handlePreviousClick = () => {
    if (canGoPrevious && !isLoading) {
      onPageChange(currentPage - 1);
    }
  };

  /**
   * Handler for "Next" button click
   */
  const handleNextClick = () => {
    if (canGoNext && !isLoading) {
      onPageChange(currentPage + 1);
    }
  };

  // Don't display pagination if it's not needed
  if (!isVisible) {
    return null;
  }

  return (
    <ShadcnPagination className={cn('mt-8', className)}>
      <PaginationContent>
        {/* Previous button */}
        <PaginationItem>
          <PaginationPrevious
            onClick={handlePreviousClick}
            className={cn(
              'cursor-pointer',
              (!canGoPrevious || isLoading) && 'pointer-events-none opacity-50'
            )}
            aria-disabled={!canGoPrevious || isLoading}
          />
        </PaginationItem>

        {/* Page numbers */}
        {pageNumbers.map((page, index) => (
          <PaginationItem key={index}>
            {page === '...' ? (
              <PaginationEllipsis />
            ) : (
              <PaginationLink
                onClick={() => handlePageClick(page)}
                isActive={page === currentPage}
                className={cn(
                  'cursor-pointer',
                  isLoading && 'pointer-events-none opacity-50'
                )}
                aria-disabled={isLoading}
              >
                {page}
              </PaginationLink>
            )}
          </PaginationItem>
        ))}

        {/* Next button */}
        <PaginationItem>
          <PaginationNext
            onClick={handleNextClick}
            className={cn(
              'cursor-pointer',
              (!canGoNext || isLoading) && 'pointer-events-none opacity-50'
            )}
            aria-disabled={!canGoNext || isLoading}
          />
        </PaginationItem>
      </PaginationContent>
    </ShadcnPagination>
  );
};