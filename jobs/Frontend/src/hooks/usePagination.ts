import { useCallback, useEffect, useState } from 'react';

export interface UsePaginationParams {
  initialPage?: number;
  maxPages?: number;
}
const usePagination = ({ initialPage, maxPages }: UsePaginationParams) => {
  const [totalPages, setTotalPages] = useState<number>(maxPages ?? 1)
  const [currentPage, setCurrentPage] = useState<number>(initialPage ?? 1);
  const incrementPage = useCallback(() => setCurrentPage((state) => state < totalPages ? state + 1 : state), [totalPages]);
  const decrementPage = useCallback(() => setCurrentPage((state) => state > 1 ? state - 1 : state), []);
  const setPage = useCallback((page: number) => setCurrentPage((state) => page > 0 && page <= totalPages ? page : state), [totalPages]);

  useEffect(() => {
    if(maxPages) setTotalPages(maxPages);
  },[maxPages])

  return {
    page: currentPage,
    decrement: decrementPage,
    increment: incrementPage,
    setPage,
    setTotalPages,
    totalPages,
  }
}

export default usePagination;
