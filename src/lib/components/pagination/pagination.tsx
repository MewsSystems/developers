import React from 'react';
import styled from 'styled-components';

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

const PaginationContainer = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 0.5rem;
  margin: 2rem 0;
`;

const PageButton = styled.button<{ isActive?: boolean }>`
  padding: 0.5rem 1rem;
  border: 1px solid ${props => props.isActive ? '#007bff' : '#dee2e6'};
  background: ${props => props.isActive ? '#007bff' : 'white'};
  color: ${props => props.isActive ? 'white' : '#007bff'};
  border-radius: 4px;
  cursor: pointer;
  transition: all 0.2s ease;

  &:hover {
    background: ${props => props.isActive ? '#007bff' : '#f8f9fa'};
  }

  &:disabled {
    background: #e9ecef;
    color: #6c757d;
    cursor: not-allowed;
    border-color: #dee2e6;
  }
`;

const PageInfo = styled.span`
  color: #6c757d;
  font-size: 0.875rem;
`;

export const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  onPageChange,
}) => {
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
      <PageButton
        onClick={handlePrevious}
        disabled={currentPage === 1}
      >
        Previous
      </PageButton>
      
      <PageInfo>
        Page {currentPage} of {totalPages}
      </PageInfo>

      <PageButton
        onClick={handleNext}
        disabled={currentPage === totalPages}
      >
        Next
      </PageButton>
    </PaginationContainer>
  );
}; 