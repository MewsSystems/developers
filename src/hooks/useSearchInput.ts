import type {ChangeEvent} from 'react';
import {useSearchParams} from 'react-router-dom';

export const useSearchInput = () => {
  const [searchParams, setSearchParams] = useSearchParams();
  const searchUrlParam = searchParams.get('search') || '';

  const onSearchInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const userSearchInputValue = e.target.value;

    if (userSearchInputValue) {
      const newParams = new URLSearchParams();
      newParams.set('search', userSearchInputValue);
      setSearchParams(newParams);
    } else {
      setSearchParams({});
    }
  };

  return {
    searchUrlParam,
    onSearchInputChange,
    clearSearch: () => setSearchParams({}),
  };
};
