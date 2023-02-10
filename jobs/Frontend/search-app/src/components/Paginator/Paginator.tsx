import { useAppSelector } from "../../app/hooks";

import { PaginatorButtons } from "./PaginatorButtons";
import { selectMoviesState } from "../../selectors/movies";
import { PaginatorContainer } from "./Paginator.styled";
import { useEffect, useState } from "react";

export const Paginator = () => {
  const state = useAppSelector(selectMoviesState);
  const currentPage = state.activePage;
  const totalPages = state.totalPages;
  const [pageNumbers, setPageNumbers] = useState<number[]>([]);

  useEffect(() => {
    let startPage = currentPage - 2;
    let endPage = currentPage + 2;
    if (startPage <= 0) {
      endPage -= startPage - 1;
      startPage = 1;
    }

    if (endPage > totalPages) {
      endPage = totalPages;
      if (endPage - 4 > 0) {
        startPage = endPage - 4;
      } else {
        startPage = 1;
      }
    }

    const numbers: number[] = [];
    for (let i = startPage; i <= endPage; i++) {
      numbers.push(i);
    }

    setPageNumbers(numbers);
  }, [currentPage, totalPages]);

  return (
    <PaginatorContainer>
      <PaginatorButtons buttonNumbers={pageNumbers} />
    </PaginatorContainer>
  );
};
