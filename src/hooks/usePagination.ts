import {useSearchParams} from 'react-router-dom';

import {updateUrlParams} from '../utils/updateUrlParams';

type UsePaginationReturnType = {
  page: number;
  setPage: (page: number) => void;
  onPageChange: (page: number) => void;
};

export const usePagination = (): UsePaginationReturnType => {
  const [urlSearchParam, setUrlSearchParam] = useSearchParams();
  const currentPage = Number(urlSearchParam.get('page')) || 1;

  const setPage = (newPage: number) => {
    const newParams = updateUrlParams({
      params: urlSearchParam,
      key: 'page',
      value: newPage.toString(),
    });

    setUrlSearchParam(newParams);
  };

  const onPageChange = (targetPageNumber: number) => {
    const pageValue = targetPageNumber > 1 ? targetPageNumber.toString() : null;
    const newParams = updateUrlParams({params: urlSearchParam, key: 'page', value: pageValue});

    setUrlSearchParam(newParams);
  };

  return {
    page: currentPage,
    setPage,
    onPageChange,
  };
};
