import {ChevronLeft} from './icons/ChevronLeft.tsx';
import {ChevronRight} from './icons/ChevronRight.tsx';
import {PageButton, PageInfo, PaginationWrapper} from './Pagination.styled.tsx';

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
      <PageButton onClick={() => onPageChange(1)} disabled={isFirstPage} isIconOnly>
        <ChevronLeft />
      </PageButton>
      <PageButton onClick={() => onPageChange(currentPage - 1)} disabled={isFirstPage}>
        Previous
      </PageButton>
      <PageInfo>
        Page {currentPage} of {totalPages}
      </PageInfo>
      <PageButton onClick={() => onPageChange(currentPage + 1)} disabled={isLastPage}>
        Next
      </PageButton>
      <PageButton onClick={() => onPageChange(totalPages)} disabled={isLastPage} isIconOnly>
        <ChevronRight />
      </PageButton>
    </PaginationWrapper>
  );
};

Pagination.displayName = 'Pagination';
