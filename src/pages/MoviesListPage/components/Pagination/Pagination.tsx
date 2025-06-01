import {NavigationButton, PaginationWrapper} from './styled';
import {useGetPages} from './hooks/useGetPages';

type PaginationProps = {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
};

export default function Pagination({currentPage, totalPages, onPageChange}: PaginationProps) {
  const pages = useGetPages({currentPage, totalPages});

  const isFirstPage = currentPage === 1;
  const isLastPage = currentPage === totalPages;

  if (totalPages <= 1) return null;

  return (
    <PaginationWrapper>
      <NavigationButton onClick={() => onPageChange(currentPage - 1)} disabled={isFirstPage}>
        {'<'}
      </NavigationButton>

      {pages.map((pageNumber) => (
        <NavigationButton
          key={pageNumber}
          onClick={() => onPageChange(pageNumber)}
          disabled={pageNumber === currentPage}
          $active={pageNumber === currentPage}
        >
          {pageNumber}
        </NavigationButton>
      ))}

      <NavigationButton onClick={() => onPageChange(currentPage + 1)} disabled={isLastPage}>
        {'>'}
      </NavigationButton>
    </PaginationWrapper>
  );
}

Pagination.displayName = 'Pagination';
