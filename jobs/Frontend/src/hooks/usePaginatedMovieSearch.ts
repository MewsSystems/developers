import usePagination from '@/hooks/usePagination';
import useSearchMovieByTitle from '@/hooks/useSearchMovieByTitle';
import { useEffect } from 'react';

const usePaginatedMovieSearch = (searchTerm?: string | null) => {
  const {setTotalPages, ...pagination} = usePagination({});
  const movies = useSearchMovieByTitle({query: searchTerm ?? '', page: pagination.page }, searchTerm === null || searchTerm === undefined);

  useEffect(() => {
    setTotalPages(1);
    pagination.setPage(1);
  }, [searchTerm]);

  useEffect(() => {
    if(movies.totalPages) {
      console.log('sets total pages to', movies.totalPages);
      setTotalPages(movies.totalPages)
    }
  }, [movies.totalPages]);

  return {
    ...movies,
    ...pagination,
  }
}

export default usePaginatedMovieSearch;
