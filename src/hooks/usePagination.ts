import {useSearchParams} from 'react-router-dom';

interface UsePaginationReturn {
  page: number;
  setPage: (page: number) => void;
  onPageChange: (page: number) => void;
}

export function usePagination(): UsePaginationReturn {
  const [searchParams, setSearchParams] = useSearchParams();
  const currentPage = Number(searchParams.get('page')) || 1;

  const setPage = (newPage: number) => {
    searchParams.set('page', newPage.toString());
    setSearchParams(searchParams);
  };

  const onPageChange = (targetPageNumber: number) => {
    const currentUrlSearchQuery = new URLSearchParams(searchParams);

    if (targetPageNumber > 1) {
      currentUrlSearchQuery.set('page', targetPageNumber.toString());
    } else {
      currentUrlSearchQuery.delete('page');
    }

    setSearchParams(currentUrlSearchQuery);
    setPage(targetPageNumber);
  };

  return {
    page: currentPage,
    setPage,
    onPageChange,
  };
}
