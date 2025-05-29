import {ChevronLeft} from './icons/ChevronLeft.tsx';
import {ChevronRight} from './icons/ChevronRight.tsx';
import {NavigationButton, PageInfo, PaginationWrapper} from './Pagination.styled.tsx';

type PaginationProps = {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
};

export const Pagination = ({currentPage, totalPages, onPageChange}: PaginationProps) => {
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
      <PageInfo>
        Page {currentPage} of {totalPages}
      </PageInfo>
      <NavigationButton onClick={() => onPageChange(currentPage + 1)} disabled={isLastPage}>
        Next
      </NavigationButton>
      <NavigationButton
        onClick={() => onPageChange(totalPages)}
        disabled={isLastPage}
        $isCompact
      >
        <ChevronRight />
      </NavigationButton>
    </PaginationWrapper>
  );
};

Pagination.displayName = 'Pagination';
