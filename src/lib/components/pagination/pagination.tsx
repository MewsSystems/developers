import { PaginationContainer, PageButton, PageInfo } from './pagination.styled';

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

export const Pagination = ({ currentPage, totalPages, onPageChange }: PaginationProps) => {
  const handlePrevious = () => {
    if (currentPage > 1) {
      onPageChange(currentPage - 1);
    }
  };

  const handleNext = () => {
    if (currentPage < totalPages) {
      onPageChange(currentPage + 1);
    }
  };

  return (
    <PaginationContainer>
      <PageButton onClick={handlePrevious} disabled={currentPage === 1}>
        Previous
      </PageButton>

      <PageInfo>
        Page {currentPage} of {totalPages}
      </PageInfo>

      <PageButton onClick={handleNext} disabled={currentPage === totalPages}>
        Next
      </PageButton>
    </PaginationContainer>
  );
};
