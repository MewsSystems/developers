import { infiniteQueryOptions, useInfiniteQuery } from '@tanstack/react-query';
import { MovieSearchResponse } from '../../types/api.ts';
import { moviesApiClient } from './lib/api-client-movies.ts';
import { QueryConfig } from './lib/react-query-config.ts';

const searchEndpoint = import.meta.env.VITE_MOVIE_DB_SEARCH_ENDPOINT;

const getMovies = (
  queryParam = '',
  pageParam = 1,
): Promise<{
  data: MovieSearchResponse;
}> => {
  return moviesApiClient.get(`${searchEndpoint}`, {
    params: {
      page: pageParam,
      query: queryParam,
    },
  });
};

export const getInfiniteMoviesQueryOptions = (queryParam: string) => {
  return infiniteQueryOptions({
    queryKey: ['movies', queryParam],
    queryFn: ({ pageParam = 1 }) => {
      return getMovies(queryParam, pageParam);
    },
    getNextPageParam: (lastPage) => {
      if (lastPage?.data?.page === lastPage?.data?.total_pages) return undefined;
      return lastPage.data.page + 1;
    },
    initialPageParam: 1,
  });
};

type UseMoviesOptions = {
  searchParam: string;
  page?: number;
  queryConfig?: QueryConfig<typeof getMovies>;
};

export const useInfiniteMovies = ({ searchParam }: UseMoviesOptions) => {
  return useInfiniteQuery({
    ...getInfiniteMoviesQueryOptions(searchParam),
  });
};

export { getMovies };
