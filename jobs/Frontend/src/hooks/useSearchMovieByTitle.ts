import { Get3SearchMovieApiArg, useGet3SearchMovieQuery } from '@/store';
import { QueryStatus } from '@reduxjs/toolkit/query';

const useSearchMovieByTitle = (searchMovieQueryParams: Get3SearchMovieApiArg, skip: boolean) => {
  const queryState = useGet3SearchMovieQuery(searchMovieQueryParams, { skip });

  return {
    totalPages: queryState.status === QueryStatus.fulfilled ? queryState.data?.total_pages ?? 1 : null,
    page: queryState.status === QueryStatus.fulfilled ? queryState.data?.page ?? 1 : null,
    movies: queryState.status === QueryStatus.fulfilled ? queryState.data?.results ?? [] : null,
    loading: queryState.status === QueryStatus.pending,
    error: queryState.status === QueryStatus.rejected ? queryState.error : null,
  }
}

export default useSearchMovieByTitle;
