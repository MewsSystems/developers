/**
 * Pagination feature types
 */
export interface PaginationState {
  currentPage: number;
  totalPages: number;
  totalResults: number;
}

export interface PaginationActions {
  setPage: (page: number) => void;
  nextPage: () => void;
  prevPage: () => void;
  goToFirstPage: () => void;
  goToLastPage: () => void;
}