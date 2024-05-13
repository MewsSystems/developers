"use client";
import { useSearchNavigation } from "@/hooks/UseSearchNavigation";
import {
  Button,
  PageIndicator,
  PaginationContainer,
} from "./MoviePaginationStyledComponents";

interface MoviePaginationProps {
  totalPages: number;
}

export default function MoviePagination({ totalPages }: MoviePaginationProps) {
  const { movieSearch, searchterm, currentPage } = useSearchNavigation();

  if (totalPages <= 1) {
    return null;
  }

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
