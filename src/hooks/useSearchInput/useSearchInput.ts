import type {ChangeEvent} from 'react';
import {useSearchParams} from 'react-router-dom';
import {updateUrlParams} from '../../utils/updateUrlParams/updateUrlParams';

export const useSearchInput = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const searchUrlParam = searchParams.get('search') || '';

  const onSearchInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const userSearchInputValue = e.target.value;
    const newParams = updateUrlParams({
      params: searchParams,
      key: 'search',
      value: userSearchInputValue || null,
    });

    setSearchParams(newParams);
  };

  return {
    searchUrlParam,
    onSearchInputChange,
    clearSearch: () =>
      setSearchParams(updateUrlParams({params: searchParams, key: 'search', value: null})),
  };
};
