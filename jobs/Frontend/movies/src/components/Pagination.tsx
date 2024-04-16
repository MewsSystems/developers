import React, { useMemo } from "react";
import styled from "styled-components";

const StyledPaginationContainer = styled.ul`
  display: flex;
  list-style-type: none;
  align-self: center;
`;

const StyledPaginationItem = styled.li<{
  $selected?: boolean;
  $isDots?: boolean;
}>`
  padding: 0px 12px;
  height: 32px;
  text-align: center;
  margin: auto 4px;
  display: flex;
  align-items: center;
  border-radius: 16px;
  font-size: var(--fs-300);
  font-family: var(--fs-serif);
  color: var(--clr-slate-800);
  min-width: 32px;

  background-color: ${(props) =>
    props.$selected ? "var(--clr-blue-300)" : "trasnparent"};

  &:hover {
    background-color: ${(props) =>
      props.$isDots ? "transparent" : "var(--clr-blue-200)"};
    cursor: ${(props) => (props.$isDots ? "default" : "pointer")};
  }
`;

export const DOTS = "...";

const range = (start: number, end: number) => {
  const length = end - start + 1;
  return Array.from({ length }, (_, idx) => idx + start);
};
const siblingCount = 1;

export interface PaginationProps {
  onPageChange: (page: number) => void;
  currentPage: number;
  totalPages: number;
}

export const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  onPageChange,
}) => {
  const paginationRange = useMemo(() => {
    // Pages count is determined as siblingCount + firstPage + lastPage + currentPage + 2*DOTS
    const totalPageNumbers = siblingCount + 5;

    /*
      If the number of pages is less than the page numbers we want to show in our
      paginationComponent, we return the range [1..totalPages]
    */
    if (totalPageNumbers >= totalPages) {
      return range(1, totalPages);
    }

    const leftSiblingIndex = Math.max(currentPage - siblingCount, 1);
    const rightSiblingIndex = Math.min(currentPage + siblingCount, totalPages);

    /*
      We do not want to show dots if there is only one position left 
      after/before the left/right page count as that would lead to a change if our Pagination
      component size which we do not want
    */
    const shouldShowLeftDots = leftSiblingIndex > 2;
    const shouldShowRightDots = rightSiblingIndex < totalPages - 2;

    const firstPageIndex = 1;
    const lastPageIndex = totalPages;

    if (!shouldShowLeftDots && shouldShowRightDots) {
      const leftItemCount = 3 + 2 * siblingCount;
      const leftRange = range(1, leftItemCount);

      return [...leftRange, DOTS, totalPages];
    }

    if (shouldShowLeftDots && !shouldShowRightDots) {
      const rightItemCount = 3 + 2 * siblingCount;
      const rightRange = range(totalPages - rightItemCount + 1, totalPages);
      return [firstPageIndex, DOTS, ...rightRange];
    }

    if (shouldShowLeftDots && shouldShowRightDots) {
      const middleRange = range(leftSiblingIndex, rightSiblingIndex);
      return [firstPageIndex, DOTS, ...middleRange, DOTS, lastPageIndex];
    }
    return [];
  }, [currentPage, totalPages]);

  if (currentPage === 0 || paginationRange.length < 2) {
    return null;
  }

  const onNext = () => {
    onPageChange(currentPage + 1);
  };

  const onPrevious = () => {
    onPageChange(currentPage - 1);
  };

  return (
    <StyledPaginationContainer>
      {currentPage !== 1 && (
        <StyledPaginationItem onClick={onPrevious}>{"<"}</StyledPaginationItem>
      )}
      {paginationRange.map((pageNumber, index) => {
        if (typeof pageNumber === "string") {
          return (
            <StyledPaginationItem key={index + pageNumber} $isDots>
              {DOTS}
            </StyledPaginationItem>
          );
        }
        return (
          <StyledPaginationItem
            key={index + pageNumber}
            onClick={() => onPageChange(pageNumber)}
            $selected={pageNumber === currentPage}
          >
            {pageNumber}
          </StyledPaginationItem>
        );
      })}
      {currentPage !== totalPages && (
        <StyledPaginationItem onClick={onNext}>{">"}</StyledPaginationItem>
      )}
    </StyledPaginationContainer>
  );
};
