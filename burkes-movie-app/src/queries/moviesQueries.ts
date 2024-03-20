import { UseQueryResult, useQuery } from '@tanstack/react-query';

import { MOVIE_DATA_URL } from '../const/endpoints';
import { MoviesApiResponse } from '../types';

const MOVIES_DATA_STALE_TIME = 10 * 60 * 1000;

const MOVIES_QUERY_KEY_BASE = ['movies'];

const MOVIES_QUERY_KEYS = {
  base: MOVIES_QUERY_KEY_BASE,
  search: (search: string, page: number) => [
    ...MOVIES_QUERY_KEY_BASE,
    'search',
    search,
    page,
  ],
};

export const fetchMovies = async (search: string, page: number) => {
  const queryUrl = `${MOVIE_DATA_URL}&query=${search ?? ''}&page=${page}`;
  const res = await fetch(queryUrl);

  const data = await res.json();

  return data;
};

export const useMoviesSearchQuery = (
  search: string,
  page: number
): UseQueryResult<MoviesApiResponse, Error> => {
  return useQuery({
    staleTime: MOVIES_DATA_STALE_TIME,
    queryKey: MOVIES_QUERY_KEYS.search(search, page),
    queryFn: () => fetchMovies(search, page),
  });
};
