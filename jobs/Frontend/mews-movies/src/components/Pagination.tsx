import React from "react";
import { PaginationProps } from "../types/PaginationInterfaces";
import styled from "styled-components";

const PaginationContainer = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 1rem;
`;

const ButtonElement = styled.button`
  font-size: 0.8rem;
  padding: 0.5rem;
  margin: 0.25rem;
  border-radius: 0.25rem;
  background-color: #4b83f1;
  border: none;
  color: white;
  cursor: pointer;

  &:hover {
    background-color: #5ca0ff;
  }

  &:disabled {
    background-color: gray;
    cursor: not-allowed;
  }

  @media (min-width: 768px) {
    font-size: 1rem;
    padding: 0.5rem 1rem;
    margin: 0.5rem;
  }
`;

const PageNumber = styled.button<{ isCurrent: boolean }>`
  font-size: 0.7rem;
  padding: 0.5rem;
  margin: 0.25rem;
  border-radius: 0.25rem;
  background-color: ${({ isCurrent }) => (isCurrent ? "#4b83f1" : "#f1f1f1")};
  border: none;
  color: ${({ isCurrent }) => (isCurrent ? "white" : "black")};
  cursor: pointer;

  &:hover {
    background-color: #5ca0ff;
    color: white;
  }

  &:disabled {
    background-color: gray;
    cursor: not-allowed;
  }

  @media (min-width: 768px) {
    font-size: 1rem;
    padding: 0.5rem 1rem;
    margin: 0.5rem;
  }
`;

const Pagination: React.FC<PaginationProps> = ({
  page,
  totalPages,
  onNextPage,
  onPrevPage,
  onPageChange,
}) => {
  const createPageNumbers = () => {
    let startPage = Math.max(1, page - 2);
    let endPage = Math.min(totalPages, page + 2);

    if (page <= 2) {
      endPage = Math.min(5, totalPages);
    } else if (page >= totalPages - 1) {
      startPage = Math.max(1, totalPages - 4);
    }

    const pages = [];
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  };

  return (
    <PaginationContainer>
      <ButtonElement onClick={() => onPageChange(1)} disabled={page === 1}>
        first
      </ButtonElement>
      <ButtonElement onClick={onPrevPage} disabled={page === 1}>
        prev
      </ButtonElement>

      {createPageNumbers().map((pageNumber) => (
        <PageNumber
          key={pageNumber}
          onClick={() => onPageChange(pageNumber)}
          isCurrent={pageNumber === page}
        >
          {pageNumber}
        </PageNumber>
      ))}

      <ButtonElement onClick={onNextPage} disabled={page === totalPages}>
        next
      </ButtonElement>
      <ButtonElement
        onClick={() => onPageChange(totalPages)}
        disabled={page === totalPages}
      >
        last
      </ButtonElement>
    </PaginationContainer>
  );
};

export default Pagination;
