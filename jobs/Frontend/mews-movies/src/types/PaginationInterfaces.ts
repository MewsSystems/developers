export interface PaginationProps {
  page: number;
  totalPages: number;
  onNextPage: () => void;
  onPrevPage: () => void;
}
