import ChevronLeft from './icons/ChevronLeft';
import ChevronRight from './icons/ChevronRight';
import {CurrentPage, NavigationButton, PaginationWrapper} from './Pagination.styled';

type PaginationProps = {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
};

export default function Pagination({currentPage, totalPages, onPageChange}: PaginationProps) {
  const isFirstPage = currentPage === 1;
  const isLastPage = currentPage === totalPages;

  if (totalPages <= 1) return null;

  return (
    <PaginationWrapper>
      <NavigationButton onClick={() => onPageChange(1)} disabled={isFirstPage} $isCompact>
        <ChevronLeft />
      </NavigationButton>
      <NavigationButton onClick={() => onPageChange(currentPage - 1)} disabled={isFirstPage}>
        Previous
      </NavigationButton>
      <CurrentPage>
        Page {currentPage} of {totalPages}
      </CurrentPage>
      <NavigationButton onClick={() => onPageChange(currentPage + 1)} disabled={isLastPage}>
        Next
      </NavigationButton>
      <NavigationButton onClick={() => onPageChange(totalPages)} disabled={isLastPage} $isCompact>
        <ChevronRight />
      </NavigationButton>
    </PaginationWrapper>
  );
}

Pagination.displayName = 'Pagination';
