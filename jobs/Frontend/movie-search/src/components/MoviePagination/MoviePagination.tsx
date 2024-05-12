"use client";
import { useSearchNavigation } from "@/hooks/UseSearchNavigation";
import styled from "styled-components";

// Pagination Container
const PaginationContainer = styled.div`
  background-color: #34495e;
  color: white;
  padding: 20px 0;
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 10px;
`;

// Button Style
const Button = styled.button`
  padding: 10px 20px;
  font-size: 16px;
  color: white;
  background-color: #2980b9; 
  border: none;
  border-radius: 5px;
  cursor: pointer;
  transition: background-color 0.3s;

  &:hover {
    background-color: #3498db; 

  &:disabled {
    background-color: #7f8c8d;
    cursor: default;
  }
`;

// Page Indicator Style
const PageIndicator = styled.span`
  font-size: 16px;
`;

interface MoviePaginationProps {
  totalPages: number;
}

export default function MoviePagination({ totalPages }: MoviePaginationProps) {
  const { movieSearch, searchterm, currentPage } = useSearchNavigation();

  return (
    <PaginationContainer>
      <Button
        disabled={currentPage === 1}
        onClick={() => movieSearch(searchterm, currentPage - 1)}
      >
        Previous
      </Button>
      <PageIndicator>
        {currentPage} of {totalPages}
      </PageIndicator>
      <Button
        disabled={currentPage === totalPages}
        onClick={() => movieSearch(searchterm, currentPage + 1)}
      >
        Next
      </Button>
    </PaginationContainer>
  );
}
