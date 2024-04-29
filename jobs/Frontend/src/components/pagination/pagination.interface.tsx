export interface PaginationProps {
  page: number;
  count: number;
  onPageChange: (page: number) => void;
}
