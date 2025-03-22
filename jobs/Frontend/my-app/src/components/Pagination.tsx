import styled from 'styled-components';
import { PaginationButton } from './PaginationButton';

const StyledPagination = styled.div`
  display: flex;
  flex-direction: row;
`;

interface PaginationProps {
  currentPage: number;
  onPageChange: (page: number) => void;
}

export const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  onPageChange,
}) => {
  const handlePrevious = () => {
    if (currentPage > 1) {
      onPageChange(currentPage - 1);
    }
  };

  const handleNext = () => {
    onPageChange(currentPage + 1);
  };

  return (
    <StyledPagination>
      <PaginationButton direction="Previous" onClick={handlePrevious} />
      <p>{currentPage}</p>
      <PaginationButton direction="Next" onClick={handleNext} />
    </StyledPagination>
  );
};
