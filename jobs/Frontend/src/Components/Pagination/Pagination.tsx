import React from 'react';
import { 
  PaginationItem,
  PaginationLayout,
  PaginationList
} from './Pagination.styled';

export interface PaginationProps {
  page: number;
  totalPages: number;
  onPageChanged: (page: string) => void;
}

export const Pagination = (props: PaginationProps) => {
  const { page, totalPages, onPageChanged } = props;

  const getPaginationRange = (currentPage: number, totalPages: number) => {
    let rangeStart: number;
    let rangeEnd: number;
  
    if (totalPages <= 5) {
      rangeStart = 1;
      rangeEnd = totalPages;
    } else if (currentPage <= 3) {
      rangeStart = 1;
      rangeEnd = 5;
    } else if (currentPage >= totalPages - 2) {
      rangeStart = totalPages - 4;
      rangeEnd = totalPages;
    } else {
      rangeStart = currentPage - 2;
      rangeEnd = currentPage + 2;
    }
  
    return Array.from({ length: rangeEnd - rangeStart + 1 }, (_, i) => rangeStart + i);
  };

  const pages = getPaginationRange(page, totalPages);

  return (
    <PaginationLayout>
      <PaginationList>
        <PaginationItem
          disabled={!(page > 1)}
          onClick={() => page > 1 && onPageChanged(`${page - 1}`)}
        >
          Prev
        </PaginationItem>
        {pages.map((p) => (
          <PaginationItem 
            key={p}
            active={p === page} 
            onClick={() => onPageChanged(`${p}`)}
          >
            {p}
          </PaginationItem>
        ))}
        <PaginationItem 
          disabled={!(page < totalPages)} 
          onClick={() => page < totalPages && onPageChanged(`${page + 1}`)}
        >
            Next
        </PaginationItem>
      </PaginationList>
    </PaginationLayout>
  );
};
