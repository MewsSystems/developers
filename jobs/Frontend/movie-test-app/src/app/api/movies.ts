import { queryOptions } from '@tanstack/react-query';
import { Meta } from '../../types/api.ts';
import Axios from 'axios';

const apiKey = import.meta.env.VITE_MOVIE_DB_API_KEY;
const baseUrl = import.meta.env.VITE_MOVIE_DB_BASE_URL;
const apiVersion = import.meta.env.VITE_MOVIE_DB_API_VERSION;
const searchEndpoint = import.meta.env.VITE_MOVIE_DB_SEARCH_ENDPOINT;

const api = Axios.create({
  baseURL: `${baseUrl}/${apiVersion}`,
});

const getMovies = (
  queryParam = 'test',
  pageParam = 1,
): Promise<{
  data: any[];
  meta: Meta;
}> => {
  return api.get(`${searchEndpoint}`, {
    params: {
      api_key: apiKey,
      page: pageParam,
      query: queryParam,
    },
  });
};

const getMoviesQueryOptions = (
  { queryParam, pageParam }: { queryParam?: string; pageParam?: number } = {
    queryParam: '',
    pageParam: 1,
  },
) => {
  return queryOptions({
    queryKey: ['movies', { queryParam, pageParam }],
    queryFn: () => getMovies(queryParam, pageParam),
  });
};

export { getMovies, getMoviesQueryOptions };
