import {useSearchParams} from 'react-router-dom';
import {useEffect} from 'react';
import type {UsePaginationWithUrlSyncParams} from './types';

export const usePaginationWithUrlSync = ({
  totalPages,
  searchQuery,
}: UsePaginationWithUrlSyncParams) => {
  const [searchParams, setSearchParams] = useSearchParams();
  const currentPage = Number(searchParams.get('page')) || 1;

  const setPage = (newPage: number) => {
    const newParams = new URLSearchParams(searchParams);
    if (newPage > 1) {
      newParams.set('page', newPage.toString());
    } else {
      newParams.delete('page');
    }
    setSearchParams(newParams);
  };

  useEffect(() => {
    if (!searchQuery && searchParams.has('page')) {
      const newParams = new URLSearchParams(searchParams);
      newParams.delete('page');
      setSearchParams(newParams);
      return;
    }

    const currentPageParam = searchParams.get('page');
    const newParams = new URLSearchParams(searchParams);

    if (totalPages > 1 && !currentPageParam) {
      newParams.set('page', '1');
      setSearchParams(newParams);
    } else if (totalPages <= 1 && currentPageParam) {
      newParams.delete('page');
      setSearchParams(newParams);
    }
  }, [totalPages, searchQuery, searchParams, setSearchParams]);

  return {
    currentPage,
    setPage,
  };
};
