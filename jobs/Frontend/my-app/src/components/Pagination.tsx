import styled from 'styled-components';
import { PaginationButton } from './PaginationButton';

const StyledPagination = styled.div`
  display: flex;
  flex-direction: row;
  gap: 1.5rem;
`;

const StyledPageNumber = styled.p`
  font-weight: 700;
`;

interface PaginationProps {
  currentPage: number;
  onPageChange: (page: number) => void;
  totalPages?: number;
}

export const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  onPageChange,
  totalPages,
}) => {
  const handlePrevious = () => {
    if (currentPage > 1) {
      onPageChange(currentPage - 1);
    }
  };

  const handleNext = () => {
    if (!totalPages || currentPage < totalPages) {
      onPageChange(currentPage + 1);
    }
  };

  return (
    <StyledPagination>
      <PaginationButton
        direction="Previous"
        onClick={handlePrevious}
        disabled={currentPage === 1}
      />
      <StyledPageNumber>{currentPage}</StyledPageNumber>
      <PaginationButton
        direction="Next"
        onClick={handleNext}
        disabled={!!totalPages && currentPage >= totalPages}
      />
    </StyledPagination>
  );
};
