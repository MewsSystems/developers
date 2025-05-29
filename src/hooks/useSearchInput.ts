import type {ChangeEvent} from 'react';
import {useSearchParams} from 'react-router-dom';

interface UseMovieSearchReturn {
  searchUrlParam: string;
  onSearchInputChange: (e: ChangeEvent<HTMLInputElement>) => void;
  clearSearch: () => void;
}

export const useSearchInput = (): UseMovieSearchReturn => {
  const [searchParams, setSearchParams] = useSearchParams();
  const searchUrlParam = searchParams.get('search') || '';

  const onSearchInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const userSearchInputValue = e.target.value;

    if (userSearchInputValue) {
      const newParams = new URLSearchParams();
      newParams.set('search', userSearchInputValue);
      setSearchParams(newParams);
    } else {
      // If search is empty, clean up the URL by removing all query params
      setSearchParams({});
    }
  };

  const clearSearch = () => {
    setSearchParams({});
  };

  return {
    searchUrlParam,
    onSearchInputChange,
    clearSearch,
  };
};
