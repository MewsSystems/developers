import {useSearchParams} from 'react-router-dom';

type UsePaginationReturn = {
  page: number;
  setPage: (page: number) => void;
  onPageChange: (page: number) => void;
};

export const usePagination = (): UsePaginationReturn => {
  const [urlSearchParam, setUrlSearchParam] = useSearchParams();
  const currentPage = Number(urlSearchParam.get('page')) || 1;

  const setPage = (newPage: number) => {
    urlSearchParam.set('page', newPage.toString());
    setUrlSearchParam(urlSearchParam);
  };

  const onPageChange = (targetPageNumber: number) => {
    const currentUrlSearchQuery = new URLSearchParams(urlSearchParam);

    if (targetPageNumber > 1) {
      currentUrlSearchQuery.set('page', targetPageNumber.toString());
    } else {
      currentUrlSearchQuery.delete('page');
    }

    setUrlSearchParam(currentUrlSearchQuery);
    setPage(targetPageNumber);
  };

  return {
    page: currentPage,
    setPage,
    onPageChange,
  };
};
